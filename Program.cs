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
            Console.WriteLine("What movie titles did you want to compare? [separate by comma, ',']");
            string[] movieTitles = Console.ReadLine().Split(','); // should return an array of strings into the movieTitles variable

            foreach(string title in movieTitles) {
                Console.WriteLine(title);
            }

            // Request to api w movieTitle:

        }

        // Methods separate to main entry point:
        static void GetAppInfo(string appName, string appAuthor) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
            Console.ResetColor(); // method exposed by the Console class to reset fg & bg colours
        }
    }
}
