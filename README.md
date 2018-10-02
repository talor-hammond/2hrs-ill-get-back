# 2hrs-ill-get-back
C# .NET async-console program that helps you pick the right movie every time (25% of the time) -- **don't waste 2hrs of your life ever again**

### The problem
* I was inspired to learn some C# and build this program after watching the *worst movie I have ever seen*: [Buried](https://www.imdb.com/title/tt1462758/) (**please don't watch it**).
* I had a stack of movies on hand, and realised having 11 or 12 separate Chrome tabs open to compare ratings & stuff was a *bit* niggly -- ultimately resulting in me wasting 2 hours of my life

### The solution
* An **async-console program** that presents brief plot / genre / rating information for any number of titles you want to search for.
    * **If you don't have any titles on hand**, you can opt to enter a genre -- and the program grabs 5 random titles out of 50 from iMDB's top-rated under that category (I used *HtmlAgilityPack* to pick-out title data for x genre from iMDB)
    
### Code-snippets
