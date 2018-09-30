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

            // bools provide a switch to different programs:
            bool enteringByTitles = false;
            bool enteringByGenre = true;
            // if the user wants to enter a genre...
            // ...method to return a relative url dynamically based on the input genre: (feed this into our scraper)

            string[] affirmatives = { "yes", "ye", "y", "yep", "yup", "yeah" };
            string[] negatives = { "n", "no", "nah", "not yet", "nope" };

            while (enteringByTitles) // TODO: need to wrap this functionality into its own program
            {
                // Gather / return all the input movie titles into an array:
                string[] movieTitles = GetMovieTitles();

                // Grab and display movie data for each movie title input:
                PresentInformationByTitles(movieTitles);

                // After the relevant info has been presented -- check the user has finished searching:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Finished searching?");
                Console.ResetColor();

                string answer = Console.ReadLine();

                if (Array.Exists(affirmatives, el => el == answer))
                {
                    // Change value that exits out of loop
                    enteringByTitles = false;
                }
                else if (Array.Exists(negatives, el => el == answer))
                {
                    // J return to continue loop
                    enteringByTitles = true;
                }
                else // if the user input was invalid / not recognised... TODO: sort functionality here
                {
                    Console.WriteLine("Input was invalid, continuing program..."); // TODO: need it to ask for prompt and re-eval 
                }
            }

            while (enteringByGenre)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("What genre were you after?");
                Console.ResetColor();
                string genre = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("Searching...");
                Console.ResetColor();

                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb(); // our disposable browser
                HtmlAgilityPack.HtmlDocument doc = web.Load($"https://www.imdb.com/search/title?genres={genre}&sort=user_rating,desc&title_type=tv_series,mini_series&num_votes=5000,&pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=f85d9bf4-1542-48d1-a7f9-48ac82dd85e7&pf_rd_r=7NT5BKR5TETJR3812F9T&pf_rd_s=right-6&pf_rd_t=15506&pf_rd_i=toptv&ref_=chttvtp_gnr_8");

                var titleNodes = doc.DocumentNode.SelectNodes("//h3[@class='lister-item-header']");

                List<string> titles = new List<string>();

                // Pushing the first 10 results on to our titles List...
                for (int i = 0; i < 10; i++)
                {
                    string titleText = titleNodes[i].SelectSingleNode(".//a").InnerText;
                    titles.Add(titleText);
                }

                // ...then
                PresentInformationByTitles(titles.ToArray());
            }
        }

        // Methods separate to main entry point ----------------------------------------------------------------------------------
        // For initialising app info:
        static void GetAppInfo(string appName, string appAuthor) // 'void' -- if no return value
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
            Console.ResetColor();
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

        // For displaying an array of movie information:
        static public void PresentInformationByTitles(string[] titles)
        {
            foreach (string title in titles)
            {
                try
                {
                    RequestMovieInformation(title).Wait();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There was an exception for '{title}': ");
                    Console.WriteLine($"{ex}");
                    Console.ResetColor();
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
            //Console.WriteLine($"Rotten Tomatoes Rating: {details["Ratings"][1]["Value"]}"); // Not every movie on db has rt rating.
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("-----------------------");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}