namespace Cinema.DataProcessor
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Data;
    using Cinema.DataProcessor.ExportDto;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.IO;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
               .Where(m => m.Rating >= rating
                        && m.Projections.Any(p => p.Tickets.Any()))
               .OrderByDescending(m => m.Rating)
               .ThenByDescending(m => m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)))
               .Take(10)
               .Select(m => new MovieWithCustomersExportDto
               {
                   MovieName = m.Title,
                   Rating = m.Rating.ToString("f2"),
                   TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                   Customers = m.Projections
                                .SelectMany(p => p.Tickets)
                                .Select(t => new CustomerExportDto
                                {
                                    FirstName = t.Customer.FirstName,
                                    LastName = t.Customer.LastName,
                                    Balance = t.Customer.Balance.ToString("f2")
                                })
                                .OrderByDescending(c => c.Balance)
                                .ThenBy(c => c.FirstName)
                                .ThenBy(c => c.LastName)
                                .ToList()
               })
               .ToList();

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var outputJson = JsonConvert.SerializeObject(movies, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Newtonsoft.Json.Formatting.Indented
            });

            return outputJson;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var sb = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(List<CustomerWithSpentMoneyExportDto>),
                                new XmlRootAttribute("Customers"));

            var customers = context.Customers
                .Where(c => c.Age >= age)
                .OrderByDescending(c => c.Tickets.Sum(t => t.Price))
                .Take(10)
                .Select(c => new CustomerWithSpentMoneyExportDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = c.Tickets.Sum(t => t.Price).ToString("f2"),
                    SpentTime = TimeSpan.FromMilliseconds(c.Tickets.Sum(t 
                        => t.Projection.Movie.Duration.TotalMilliseconds)).ToString(@"hh\:mm\:ss")
                })
                .ToList();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}