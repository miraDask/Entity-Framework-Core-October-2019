namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string Error = "Invalid Data";
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var dtos = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto) || dto.Cells.Length == 0 || !dto.Cells.All(x => IsValid(x)))
                {
                    sb.AppendLine(Error);
                    continue;
                }

                var validCells = new List<CellDto>();
                foreach (var cDto in dto.Cells)
                {
                    if (context.Cells.Any(x => x.CellNumber == cDto.CellNumber) )
                    {
                        continue;
                    }

                    validCells.Add(cDto);
                }

                if (dto.Cells.Length != validCells.Count)
                {
                    sb.AppendLine(Error);
                    continue;
                }

                var department = new Department
                {
                    Name = dto.Name,
                    Cells = dto.Cells.Select(x => new Cell { CellNumber = x.CellNumber, HasWindow = x.HasWindow}).ToArray(),
                };

                context.Add(department);
                context.SaveChanges();
                sb.AppendLine($"Imported {dto.Name} with {dto.Cells.Length} cells");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var dtos = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto) || !dto.Mails.All(x => IsValid(x)))
                {
                    sb.AppendLine(Error);
                    continue;
                }

                var prisoner = new Prisoner
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    Bail = dto.Bail,
                    IncarcerationDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = dto.ReleaseDate == null ? null : (DateTime?)DateTime.ParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CellId = dto.CellId,
                    Mails = dto.Mails.Select(x => new Mail { Description = x.Description, Sender = x.Sender, Address = x.Address }).ToArray()
                };

                context.Add(prisoner);
                context.SaveChanges();
                sb.AppendLine($"Imported {dto.FullName} {dto.Age} years old");

            };

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(List<OfficerDto>),
                             new XmlRootAttribute("Officers"));

            List<OfficerDto> dtos;

            using (var reader = new StringReader(xmlString))
            {
                dtos = (List<OfficerDto>)xmlSerializer.Deserialize(reader);

                foreach (var dto in dtos)
                {

                    var isValidPosition = Enum.TryParse<Position>(dto.Position, false, out var position);
                    var isValidWeapon = Enum.TryParse<Weapon>(dto.Weapon, false, out var weapon);
                    var department = context.Departments.Where(x => x.Id == dto.DepartmentId).FirstOrDefault();

                    if (!IsValid(dto) || !isValidPosition || !isValidWeapon)
                    {
                        sb.AppendLine(Error);
                        continue;
                    }

                    var officer = new Officer
                    {
                        FullName = dto.Name,
                        Salary = dto.Money,
                        Position = position,
                        Weapon = weapon,
                        Department = department,
                        OfficerPrisoners = dto.Prisoners.Select(x => new OfficerPrisoner
                        {
                            PrisonerId = x.Id
                        })
                        .ToArray()
                    };

                    context.Add(officer);
                    context.SaveChanges();
                    sb.AppendLine($"Imported {dto.Name} ({dto.Prisoners.Length} prisoners)");

                }
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}