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
            string name = "Talor";
            int age = 22;

            //Console.WriteLine("Hello " + name);
            Console.WriteLine("{0} is age: {1}", name, age);
        }
    }
}
