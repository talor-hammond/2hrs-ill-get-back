# 2hrs-ill-get-back
C# .NET async-console program that helps you pick the right movie every time (25% of the time)

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
If the user wants top-rated picks by genre, the following program path is run:
```c#
else if (enteringBy == "genre")
            {
                bool isSearching = true;
                while (isSearching) {
                    // Check what genre that user wants information for:
                    string genre = DetermineGenre();
                    
                    // Begin grabbing random titles by genre from iMDBB's top-rated and store them as an array:
                    PrintRedMessageToConsole("Searching...");
                    string[] titles = FetchUniqueTitlesByGenre(genre);
                    
                    // ...then
                    PresentInformationByTitles(titles);

                    // Check the user has finished searching:
                    isSearching = CheckUserFinished();
                }
            }
```
Once a genre has been determined via the `DetermineGenre()` method, the program fetches unique top-rated titles by genre from iMDB. Used *HtmlAgilityPack* to scrape ~50 titles from IMDB; wrote `GenerateUniqueIndexes(int max)` to take the length of all the titles, and randomly pick out 5.
```c#
        static string[] FetchUniqueTitlesByGenre(string genre)
        {
            List<string> titles = new List<string>(); // initialising our list of titles

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb(); // instance of our disposable / headless browser
            HtmlAgilityPack.HtmlDocument doc = web.Load($"https://www.imdb.com/search/title?genres={genre}&sort=user_rating,desc&title_type=tv_series,mini_series&num_votes=5000,&pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=f85d9bf4-1542-48d1-a7f9-48ac82dd85e7&pf_rd_r=7NT5BKR5TETJR3812F9T&pf_rd_s=right-6&pf_rd_t=15506&pf_rd_i=toptv&ref_=chttvtp_gnr_8");

            var titleNodes = doc.DocumentNode.SelectNodes("//h3[@class='lister-item-header']");

            int[] randomIndexes = GenerateUniqueIndexes(titleNodes.Count - 1); // generating an array of random indexes between 0 and one less than titleNodes.

            foreach (int index in randomIndexes)
            {
                string titleText = titleNodes[index].SelectSingleNode(".//a").InnerText; // this element has the title name we are after.
                titles.Add(titleText);
            }

            return titles.ToArray();
        }
```
The `GenerateUniqueIndexes(int max)` method looks a bit like this...
```c#
// For generating 5 random indexes between 0 and the specified max.
        static int[] GenerateUniqueIndexes(int max)
        {
            List<int> indexes = new List<int>();
            Random rnd = new Random(); // gives us access to the `.Next(min, max)` method

            while (indexes.Count < 5) // because we only want to create 5 unique indexes to iterate through.
            {
                int randomIndex = rnd.Next(0, max);
                if (!indexes.Contains(randomIndex))
                    indexes.Add(randomIndex);  // only if the index doesn't exist in the list already
            }

            return indexes.ToArray();
        }
```
Once 5 unique titles from IMDB's top-rated section have been grabbed, the program makes `.get()` requests to [OMDb API](http://www.omdbapi.com/):
```c#
                    string[] titles = FetchUniqueTitlesByGenre(genre);
                    
                    // ...then
                    PresentInformationByTitles(titles);
```

Requests for movie information are made for each title in the array of titles fed through:
```c#
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
```

This method takes a title and makes dynamic requests -- we tell the program `await` to make sure the asynchronous request resolves w the relevant data before continuing:
```c#
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
```

### Conclusion
Had an awesome time using C# on the .NET framework for the first time --  especially creating a cool little executable / program that has some use.
* The language itself felt different syntactically; but much the same in functionality as any other functional programming language, so building the logic for the program felt natural
* First go with web-scraping (plz don't arrest me) went super smoothly thanks to `HtmlAgilityPack` -- `puppeteer` is a relative library for JavaScript / Node that can be used in the same way
