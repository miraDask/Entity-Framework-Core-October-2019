namespace BookShop
{
    using Data;
    using System;
    using System.Text;
    using System.Linq;
    using BookShop.Models.Enums;

    public class StartUp
    {
        public static void Main()
        {
            var context = new BookShopContext();
        
            using (context)
            {
                // Problem 1 :
                //var command = Console.ReadLine();
                //var books = GetBooksByAgeRestriction(context, command);
                //Console.WriteLine(books);

                //Problem 2:
                //var books = GetGoldenBooks(context);
                //Console.WriteLine(books);

                //Problem 3:
                var books = GetBooksByPrice(context);
                Console.WriteLine(books);
            }
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

        //Problem 2:
        public static string GetGoldenBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold)
                .Where(b => b.Copies < 5000)
                .Select(b => new
                {
                    b.BookId,
                    b.Title
                })
                .OrderBy(b => b.BookId)
                .ToList();

            books.ForEach(b => sb.AppendLine(b.Title));
    
            return sb.ToString().TrimEnd();
        }

        //Problem 3:
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToList();

            books.ForEach(b => sb.AppendLine($"{b.Title} - ${b.Price:f2}"));

            return sb.ToString().TrimEnd();
        }
    }
}
