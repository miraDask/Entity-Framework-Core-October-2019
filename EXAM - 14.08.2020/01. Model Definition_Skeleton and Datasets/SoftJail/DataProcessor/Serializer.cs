namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var officers = context
                .Prisoners
                .ToArray()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new 
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(x => new
                    {
                        OfficerName = x.Officer.FullName,
                        Department = x.Officer.Department.Name
                    })
                    .OrderBy(x => x.OfficerName),
                    TotalOfficerSalary = Math.Truncate(100 * p.PrisonerOfficers.Sum(x => x.Officer.Salary)) / 100
        })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();
            var outputJson = JsonConvert.SerializeObject(officers, Newtonsoft.Json.Formatting.Indented);
            return outputJson;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonersNamesCollection = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries);

            var prisoners = context
                .Prisoners
                .ToArray()
                .Where(p => prisonersNamesCollection.Contains(p.FullName))
                .Select(p => new PrisonerDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(x => new MessageDto
                    {
                        Description = ReverseText(x.Description)
                    }).ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();


            var xmlSerializer = new XmlSerializer(typeof(PrisonerDto[]),
                                            new XmlRootAttribute("Prisoners"));



            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, prisoners, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        private static string ReverseText(string text)
        {
            string[] words = text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                var wordToCharArray = words[i].ToCharArray();
                Array.Reverse(wordToCharArray);
                words[i] = String.Join(string.Empty, wordToCharArray);
            }

            Array.Reverse(words);
            return String.Join(" ", words);
        }
    }
}