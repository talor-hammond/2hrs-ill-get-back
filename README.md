# 2hrs-ill-get-back
C# .NET async-console program that helps you pick the right movie every time (25% of the time) -- **don't waste 2hrs of your life ever again**

### The problem
* I was inspired to learn some C# and build this program after watching the *worst movie I have ever seen*: [Buried](https://www.imdb.com/title/tt1462758/) (**please don't watch it**).
* I had a stack of movies on hand, and realised having 11 or 12 separate Chrome tabs open to compare ratings & stuff was a *bit* niggly -- ultimately resulting in me wasting 2 hours of my life

### The solution
* An **async-console program** that presents brief plot / genre / rating information for any number of titles you want to search for.
    * **If you don't have any titles on hand**, you can opt to enter a genre -- and the program grabs 5 random titles out of 50 from iMDB's top-rated under that category (I used *HtmlAgilityPack* to pick-out title data for x genre from iMDB)
    
### Code-snippets
The program begins by asking users whether they want to search a list of movie titles, or randomly select information from a specific genre...
```c#
// Ask the user what type of search they want to make:
            PrintRedMessageToConsole("Did you want info for titles you wanted to compare?");
            PrintRedMessageToConsole("Or did you want top-rated picks by genre?");
            Console.WriteLine("Try entering 'titles', or 'genre'...");
            Console.WriteLine();

            string enteringBy = CheckWhatUserWants();
```
... the `CheckWhatUserWants()` method returns a string into the `enteringBy` variable; it calls itself recursively if the user fails to enter a valid input:
```c#
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
                return CheckWhatUserWants(); // Checks recursively until a valid input is made.
        }
```
