﻿namespace BookShop
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
                //var books = GetBooksByPrice(context);
                //Console.WriteLine(books);

                //Problem 4:
                //var year = int.Parse(Console.ReadLine());
                //var books = GetBooksNotReleasedIn(context, year);
                //Console.WriteLine(books);

                var input = Console.ReadLine();
                var books = GetBooksByCategory(context, input);
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
    }
}
