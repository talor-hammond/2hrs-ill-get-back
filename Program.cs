using System; // makes the 'Console' class available to the program
using System.Collections.Generic; // Makes 'List' available

namespace hrsillnevergetback
{
    // Primary class:
    class Program
    {
        // 'Entry-point' method
        public static void Main(string[] args) // 'PascalCase' for methods
        {
            GetAppInfo("2hrs I Won't Need Back", "Talor Hammond");

            // Gather / return all the input movie titles into an array
            //Console.WriteLine("What movie titles did you want to compare? [separate by comma, ',']");
            //string[] movieTitles = Console.ReadLine().Split(','); // should return an array of strings into the movieTitles variable

            //foreach (string title in movieTitles)
            //{
            //    Console.WriteLine(title);
            //}

            // Gather / return all the input movie titles into an array:
            string[] movieTitles = GetMovieTitles(); // need to store this value in a variable, loop through w requests (figure out how to do in parallel like Promise.All()

            foreach (string title in movieTitles)
            {
                Console.WriteLine(title);
            }
            // Request to api w movieTitle:

        }

        // Methods separate to main entry point:
        static void GetAppInfo(string appName, string appAuthor) { // 'void' -- if no return value
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
            Console.ResetColor(); // method exposed by the Console class to reset fg & bg colours
        }

        static string[] GetMovieTitles() { // returns an array of strings
            Console.WriteLine("What movie titles did you want to compare? [enter 'q' when finished]");
            List<string> movieTitles = new List<string>();

            bool stillEnteringTitles = true;
            while (stillEnteringTitles)
            {
                string input = Console.ReadLine();

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
    }
}
