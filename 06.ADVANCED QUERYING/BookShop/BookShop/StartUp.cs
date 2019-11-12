namespace BookShop
{
    using Data;
    using System;
    using System.Text;
    using System.Linq;
    using BookShop.Models.Enums;
    using System.Globalization;

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
                //var books = GetBooksByPrice(context);
                //Console.WriteLine(books);

                //Problem 4:
                //var year = int.Parse(Console.ReadLine());
                //var books = GetBooksNotReleasedIn(context, year);
                //Console.WriteLine(books);

                //Problem 5:
                //var input = Console.ReadLine();
                //var books = GetBooksByCategory(context, input);
                //Console.WriteLine(books);

                //Problem 6:
                //var date = Console.ReadLine();
                //var books = GetBooksReleasedBefore(context, date);
                //Console.WriteLine(books);

                //Problem 7:
                //var input = Console.ReadLine();
                //var authors = GetAuthorNamesEndingIn(context, input);
                //Console.WriteLine(authors);

                //Problem 8:
                //var input = Console.ReadLine();
                //var authors = GetBookTitlesContaining(context, input);
                //Console.WriteLine(authors);

                //Problem 9:
                //var input = Console.ReadLine();
                //var books = GetBooksByAuthor(context, input);
                //Console.WriteLine(books);

                //Problem 10:
                //var lengthCheck = int.Parse(Console.ReadLine());
                //var bookCount = CountBooks(context, lengthCheck);
                //Console.WriteLine(bookCount);
                //Console.WriteLine($"There are {bookCount} books with longer title than {lengthCheck} symbols");

                //Problem 11:
                //var authors = CountCopiesByAuthor(context);
                //Console.WriteLine(authors);

                //Problem 12:
                //var totalProfitByCategory = GetTotalProfitByCategory(context);
                //Console.WriteLine(totalProfitByCategory);

                //Problem 13:
                var mostRecentBooks = GetMostRecentBooks(context);
                Console.WriteLine(mostRecentBooks);
            }
        }

        //Problem 1:
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.AgeRestriction.ToString().ToUpper() == command.ToUpper())
                .Select(b => new
                {
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

        //Problem 4:
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
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

        //Problem 5:
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var sb = new StringBuilder();
            var categories = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var books = context
                .Books
                .Where(b => b.BookCategories
                             .Select(bc => new
                             {
                                 bc.Category.Name
                             }).Any(bc => categories.Contains(bc.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            books.ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd();
        }

        //Problem 6:
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();
            var dateFormat = "dd-MM-yyyy";

            var books = context
                .Books
                .OrderByDescending(b => b.ReleaseDate)
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, dateFormat, CultureInfo.InvariantCulture))
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            books.ForEach(b => sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}"));

            return sb.ToString().TrimEnd();
        }

        //Problem 7:
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var autors = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                })
                .OrderBy(a => a.FullName)
                .ToList();

            autors.ForEach(a => sb.AppendLine(a.FullName));

            return sb.ToString().TrimEnd();
        }

        //Problem 8:
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            books.ForEach(b => sb.AppendLine(b));

            return sb.ToString().TrimEnd();
        }

        //Problem 9:
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorName = b.Author.FirstName + " " + b.Author.LastName
                })
                .ToList();

            books.ForEach(b => sb.AppendLine($"{b.Title} ({b.AuthorName})"));

            return sb.ToString().TrimEnd();
        }

        //Problem 10:
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList();

            return books.Count;
        }

        //Problem 11:
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var authors = context
                .Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToList();

            authors.ForEach(a => sb.AppendLine($"{a.FullName} - {a.TotalCopies}"));

            return sb.ToString().TrimEnd();
        }

        //Problem 12:
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks
                                   .Select(cb => cb.Book.Price * cb.Book.Copies)
                                   .Sum()
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToList();

            categories.ForEach(c => sb.AppendLine($"{c.Name} ${c.TotalProfit:f2}"));

            return sb.ToString().TrimEnd();
        }

        //Problem 13:
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecsentBooks = c.CategoryBooks
                                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                                        .Take(3)
                                        .Select(cb => new
                                        {
                                            cb.Book.Title,
                                            cb.Book.ReleaseDate
                                        })
                                        .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList();

            categories.ForEach(c =>
            {
                sb.AppendLine($"--{c.Name}");
                c.MostRecsentBooks.ForEach(b => sb.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})"));
                
            });

            return sb.ToString().TrimEnd();
        }
    }
}
