namespace BookShop
{
    using Data;
    using System;
    using System.Text;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            var context = new BookShopContext();
            
            var command = Console.ReadLine();
            var books = GetBooksByAgeRestriction(context, command);
            Console.WriteLine(books);
        }

        //Problem 1:
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var sb = new StringBuilder();

                var books = context
                    .Books
                    .Where(b => b.AgeRestriction.ToString().ToUpper() == command.ToUpper())
                    .Select(b => new { 
                        b.Title
                    })
                    .OrderBy(b => b.Title)
                    .ToList();

                books.ForEach(b => sb.AppendLine(b.Title));

            return sb.ToString().TrimEnd();
        }
    }
}
