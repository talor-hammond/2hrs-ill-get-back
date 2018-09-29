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
            GetMovieTitles();
           
            // Request to api w movieTitle:

        }

        // Methods separate to main entry point:
        static void GetAppInfo(string appName, string appAuthor) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
            Console.ResetColor(); // method exposed by the Console class to reset fg & bg colours
        }

        static void GetMovieTitles() {
            Console.WriteLine("What movie titles did you want to compare? [enter 'q' when finished]");
            bool stillEnteringTitles = true;
            List<string> movieTitles = new List<string>();

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

            foreach (string title in movieTitles.ToArray()) {
                Console.WriteLine(title);
            }
        }
    }
}
