using System; // makes the 'Console' class available to the program

namespace hrsillnevergetback
{
    // Primary class:
    class Program
    {
        // 'Entry-point' method
        public static void Main(string[] args) // 'PascalCase' for methods
        {
            GetAppInfo("2hrs I Won't Need Back", "Talor Hammond");

            // Gather movie title from console input:
            Console.WriteLine("What movie title did you want to check?");
            string movieTitle = Console.ReadLine();
            Console.WriteLine("Gathering rating & details for '{0}'...", movieTitle);

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
