namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(List<BookDto>),
                             new XmlRootAttribute("Books"));

            List<BookDto> booksDtos;

            using (var reader = new StringReader(xmlString))
            {
                booksDtos = (List<BookDto>)xmlSerializer.Deserialize(reader);

                foreach (var dto in booksDtos)
                {
                    if (!IsValid(dto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var book = new Book
                    {
                        Name = dto.Name,
                        Genre = (Genre)dto.Genre,
                        Price = dto.Price,
                        Pages = dto.Pages,
                        PublishedOn = DateTime.ParseExact(dto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                    };

                    context.Books.Add(book);
                    sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
                }

                context.SaveChanges();
            }


            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var authorsDtos = JsonConvert.DeserializeObject<AuthorDto[]>(jsonString);

            foreach (var dto in authorsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Authors.Any(x => x.Email == dto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validBooks = new List<BookIdDto>();
                
                foreach (var book in dto.Books)
                {
                    if (!book.Id.HasValue || !context.Books.Any(x => x.Id == book.Id)) 
                    {
                        continue;
                    }

                    validBooks.Add(book);
                }

                if (validBooks.Count == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone
                };

                context.Authors.Add(author);
                context.SaveChanges();

                foreach (var book in validBooks)
                {
                    var authorBook = new AuthorBook
                    {
                        AuthorId = author.Id,
                        BookId = (int)book.Id
                    };

                    context.AuthorsBooks.Add(authorBook);
                }

                context.SaveChanges();
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, $"{author.FirstName} {author.LastName}", validBooks.Count));
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}