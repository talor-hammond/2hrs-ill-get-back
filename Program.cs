using System; // makes the 'Console' class available to the program

namespace hrsillnevergetback
{
    // Primary class:
    class Program
    {
        // 'Entry-point' method
        public static void Main(string[] args) //  'static'; no 'instances'
        {
            // Variable declaration: type is prefixed...
            string appName = "2hrs I Won't Need Back";
            string appAuthor = "Talor Hammond";

            //Console.WriteLine("Hello " + name);
            Console.WriteLine("{0}, by {1}", appName, appAuthor);
        }
    }
}
