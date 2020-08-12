namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .OrderByDescending(x => x.AuthorsBooks.Count)
                .ThenBy(x => $"{x.FirstName} {x.LastName}")
                .Select(x => new
                {
                    AuthorName = $"{x.FirstName} {x.LastName}",
                    Books = x.AuthorsBooks.OrderByDescending(b => b.Book.Price).Select(y => new
                    {
                        BookName = y.Book.Name,
                        BookPrice = y.Book.Price.ToString("f2")
                    })
                    
                }).ToList();

            var outputJson = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return outputJson;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {

            var books = context.Books
                .Where(x => x.Genre == Genre.Science && x.PublishedOn < date)
                .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Take(10)
                .Select(x => new ExpBookDto
                {
                    Pages = x.Pages,
                    Name = x.Name,
                    Date = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
                })
                .ToList();
            
            var xmlSerializer = new XmlSerializer(typeof(List<ExpBookDto>),
                                            new XmlRootAttribute("Books"));
            


            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, books, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}