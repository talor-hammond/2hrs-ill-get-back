using System; // makes the 'Console' class available to the program

namespace hrsillnevergetback
{
    // Primary class:
    class Program
    {
        // Function that takes a movieTitle and returns a rating & details:

        // 'Entry-point' method
        public static void Main(string[] args) //  'static'; no 'instances'
        {

            // Initialising variables:
            string appName = "2hrs I Won't Need Back"; // Variable declaration: type is prefixed...
            string appAuthor = "Talor Hammond";
            // API key?

            // app-header
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
            Console.ResetColor(); // method exposed by the Console class to reset fg & bg colours

            // Gather movie title from console input:
            Console.WriteLine("What movie title did you want to check?");
            string movieTitle = Console.ReadLine();
            Console.WriteLine("Gathering rating & details for {0}...", movieTitle);

            // Request to api w movieTitle:
        }
    }
}
