using System; // makes the 'Console' class available to the program
using System.Collections.Generic; // Makes 'List' obj available
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace hrsillnevergetback
{
    // Primary class:
    class Program
    {
        // 'Entry-point' method
        public static void Main(string[] args) // 'PascalCase' for methods
        {
            GetAppInfo("2hrs I Won't Need Back", "Talor Hammond");

            // Gather / return all the input movie titles into an array:
            string[] movieTitles = GetMovieTitles(); // need to store this value in a variable, loop through w requests (figure out how to do in parallel like Promise.All()

            foreach (string title in movieTitles)
            {
                try
                {
                    RequestMovieInformation(title).Wait();
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"There was an exception: {ex}");
                }
            }
            // Request to api w movieTitle:

        }

        // Methods separate to main entry point ----------------------------------------------------------------------------------

        // For initialising app info:
        static void GetAppInfo(string appName, string appAuthor) // 'void' -- if no return value
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
            Console.ResetColor(); // method exposed by the Console class to reset fg & bg colours
        }

        // For getting an array of movie titles from user input:
        static string[] GetMovieTitles() // returns an array of strings
        {
            Console.WriteLine("What movie titles did you want to compare? [enter 'q' when finished]");
            List<string> movieTitles = new List<string>();

            bool stillEnteringTitles = true;
            while (stillEnteringTitles)
            {
                string input = Console.ReadLine(); // prompt the user for new input at the start of each loop

                if (input != "q")
                { // If the user hasn't exit'd the program w 'q', add the input to our array
                    movieTitles.Add(input);
                }
                else
                {
                    stillEnteringTitles = false;
                }
            }

            return movieTitles.ToArray(); // .ToArray(): method exposed by List obj
        }

        // For gathering relevant movie information from omdb api:
        static public async Task RequestMovieInformation(string title)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Fetching information for '{title}'...");
            Console.ResetColor();

            string url = "http://www.omdbapi.com/";
            string apiKey = "4edeff6c";
            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync($"{url}?t={title}&apikey={apiKey}");

            JObject movieDetails = JObject.Parse(response);

            // Presenting our parsed-data:
            Console.WriteLine($"Plot: {movieDetails["Plot"]}");
        }
    }
}