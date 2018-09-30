using System; // makes the 'Console' class available to the program
using System.Collections.Generic; // Makes 'List' obj available
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Hoursillgetback
{
    // Primary class:
    class Program
    {
        // 'Entry-point' method; invoked when Program is initialised
        public static void Main(string[] args)
        {
            bool isRunning = true;
            string[] affirmatives = { "yes", "ye", "y", "yep", "yup", "yeah" };
            string[] negatives = { "n", "no", "nah", "not yet", "nope" };

            GetAppInfo("MoviePicker", "T. Hammond");

            while (isRunning) {
                // Gather / return all the input movie titles into an array:
                string[] movieTitles = GetMovieTitles();
                
                // Grab and display movie data for each movie title input:
                foreach (string title in movieTitles)
                {
                    try
                    {
                        RequestMovieInformation(title).Wait();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Had an error finding stuff for '{title}'!");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Finished searching?");
                Console.ResetColor();

                string answer = Console.ReadLine();
                
                if (Array.Exists(affirmatives, el => el == answer))
                {
                    // Change value that exits out of loop
                    isRunning = false;
                }
                else if (Array.Exists(negatives, el => el == answer))
                {
                    // J return to continue loop
                    isRunning = true;
                }
                else // if the user input was invalid / not recognised...
                {
                    Console.WriteLine("Oops");
                }
            }
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
            WriteMovieDetails(movieDetails);
        }

        // For writing the relevant movie details to the console:
        static public void WriteMovieDetails(JObject details)
        {
            // Plot:
            Console.WriteLine($"Plot: {details["Plot"]}");
            Console.WriteLine();
            // Genre:
            Console.WriteLine($"Genre: {details["Genre"]}");
            Console.WriteLine();
            // Ratings:
            Console.WriteLine($"iMDB Rating: {details["Ratings"][0]["Value"]}");
            //Console.WriteLine($"Rotten Tomatoes Rating: {details["Ratings"][1]["Value"]}"); // Not every movie on db has rt rating.
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("-----------------------");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}