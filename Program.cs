using System; // makes the 'Console' class available to the program
using System.Collections.Generic; // Makes 'List' obj available
using System.Net.Http;
using System.Xml;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

// TODO: Web-scrape top movie / series titles by genre (randomly pick 3 of top 10) -- get information for all of the titles that come back:

namespace Hoursillgetback
{
    // Primary class:
    class Program
    {
        // 'Entry-point' method; invoked when Program is initialised
        public static void Main(string[] args)
        {
            GetAppInfo("MoviePicker", "T. Hammond");

            // Ask the user what type of search they want to make:
            PrintRedMessageToConsole("Did you want info for titles you wanted to compare?");
            PrintRedMessageToConsole("Or did you want top-rated picks by genre?");
            Console.WriteLine("Try entering 'titles', or 'genre'...");
            Console.WriteLine();

            string enteringBy = CheckWhatUserWants();

            // Init program:
            if (enteringBy == "titles")
            {
                bool isSearching = true;
                while (isSearching) {
                    // Gather / return all the input movie titles into an array:
                    string[] movieTitles = GetMovieTitles();
                    
                    // Grab and display movie data for each movie title input:
                    PresentInformationByTitles(movieTitles);

                    // Check the user has finished searching:
                    isSearching = CheckUserFinished();
                }
            }
            else if (enteringBy == "genre")
            {
                bool isSearching = true;
                while (isSearching) {
                    Console.WriteLine();
                    PrintRedMessageToConsole("What genre were you after?");
                    string genre = Console.ReadLine();
                    
                    // Begin grabbing random titles by genre from iMDBB and store them as an array:
                    PrintRedMessageToConsole("Searching...");
                    string[] titles = FetchUniqueTitlesByGenre(genre);
                    
                    // ...then
                    PresentInformationByTitles(titles);

                    // Check the user has finished searching:
                    isSearching = CheckUserFinished();
                }
            }

        }

        // Methods separate to main entry point ----------------------------------------------------------------------------------
        // For initialising app info:
        static void GetAppInfo(string appName, string appAuthor) // 'void' -- if no return value
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
            Console.ResetColor();
            Console.WriteLine();
        }

        // For writing a red-coloured console message to the console:
        static void PrintRedMessageToConsole(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // For getting an array of movie titles from user input:
        static string[] GetMovieTitles() // returns an array of strings
        {
            Console.WriteLine();
            PrintRedMessageToConsole("What movie titles did you want to compare? [enter 'q' when finished]");
            List<string> movieTitles = new List<string>();

            bool stillEnteringTitles = true;
            while (stillEnteringTitles)
            {
                string input = Console.ReadLine(); // prompt the user for new input at the start of each loop

                if (input != "q") // If the user hasn't exit'd the program w 'q', add the input to our array
                    movieTitles.Add(input);
                else
                    stillEnteringTitles = false;
            }

            return movieTitles.ToArray(); // .ToArray(): method exposed by List obj
        }

        // For displaying an array of movie information:
        static public void PresentInformationByTitles(string[] titles)
        {
            foreach (string title in titles)
            {
                try
                {
                    RequestMovieInformation(title).Wait();
                }
                catch
                {
                    PrintRedMessageToConsole($"Couldn't manage to find any data for '{title}'!");
                    Console.WriteLine();
                    //Console.WriteLine($"{ex}"); ...for Exception ex 
                }
            }
        }

        // For gathering specific movie information by title from omdb api:
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
            Console.WriteLine();

            PrintRedMessageToConsole("-----------------------");
            Console.WriteLine();
        }

        // For generating 5 random indexes between 0 and the specified max.
        static int[] GenerateUniqueIndexes(int max)
        {
            List<int> indexes = new List<int>();
            Random rnd = new Random();

            while (indexes.Count < 5)
            {
                int randomIndex = rnd.Next(0, max);
                if (!indexes.Contains(randomIndex))
                    indexes.Add(randomIndex);
            }

            return indexes.ToArray();
        }

        // For grabbing five unique titles from iMDB's top-rated by genre:
        static string[] FetchUniqueTitlesByGenre(string genre)
        {
            List<string> titles = new List<string>();

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb(); // our disposable browser
            HtmlAgilityPack.HtmlDocument doc = web.Load($"https://www.imdb.com/search/title?genres={genre}&sort=user_rating,desc&title_type=tv_series,mini_series&num_votes=5000,&pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=f85d9bf4-1542-48d1-a7f9-48ac82dd85e7&pf_rd_r=7NT5BKR5TETJR3812F9T&pf_rd_s=right-6&pf_rd_t=15506&pf_rd_i=toptv&ref_=chttvtp_gnr_8");

            var titleNodes = doc.DocumentNode.SelectNodes("//h3[@class='lister-item-header']");

            int[] randomIndexes = GenerateUniqueIndexes(titleNodes.Count - 1); // generating an array of random indexes between 0 and one less than titles.

            foreach (int index in randomIndexes)
            {
                string titleText = titleNodes[index].SelectSingleNode(".//a").InnerText;
                titles.Add(titleText);
            }

            return titles.ToArray();
        }

        // Method which returns a bool to check whether a user wants to keep searching:
        static bool CheckUserFinished()
        {
            // Arrays of answers to check conditionally:
            string[] affirmatives = { "yes", "ye", "y", "yep", "yup", "yeah" };
            string[] negatives = { "n", "no", "nah", "not yet", "nope" };

            PrintRedMessageToConsole("Finished searching?");
            string answer = Console.ReadLine();

            if (Array.Exists(affirmatives, el => el == answer))
            {
                return false; // exits out of the loop.
            }
            else if (Array.Exists(negatives, el => el == answer))
            {
                return true;
            }
            else // if the user input was invalid / not recognised...
            {
                PrintRedMessageToConsole("Input was invalid, continuining program"); // TODO: need it to ask for prompt and re-eval 
                return true;
            }
        }

        // For determing what type of search the user wants to make:
        static string CheckWhatUserWants()
        {
            string input = Console.ReadLine().ToLower();

            if (input == "title" || input == "titles")
                return "titles";
            else if (input == "genre")
                return "genre";
            else
                PrintRedMessageToConsole("Oops, please try 'title', or 'genre'");
                return CheckWhatUserWants(); // If they failed to enter the correct string, check again until a valid input is made!
        }
    }
}