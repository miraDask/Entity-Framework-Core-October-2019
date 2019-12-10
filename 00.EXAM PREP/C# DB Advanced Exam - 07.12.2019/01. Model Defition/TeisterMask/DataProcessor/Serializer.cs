namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(ProjectWithTasksExportDto[]),
                                new XmlRootAttribute("Projects"));

            var projects = context.Projects
                .Where(p => p.Tasks.Any())
                .OrderByDescending(p => p.Tasks.Count)
                .ThenBy(p => p.Name)
                .Select(p => new ProjectWithTasksExportDto()
                {
                    TasksCount = p.Tasks.Count.ToString(),
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate != null ? "Yes" : "No",
                    Tasks = p.Tasks
                             .OrderBy(t => t.Name)
                             .Select(t => new TaskXmlExportDto()
                             {
                                 Name = t.Name,
                                 Label = t.LabelType.ToString()
                             })
                            .ToArray()
                })
                .ToArray();

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, projects, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .OrderByDescending(e => e.EmployeesTasks.Count(et => et.Task.OpenDate >= date))
                .ThenBy(e => e.Username)
                .Select(e => new EmploeeWithTasksExportDto()
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                             .Where(et => et.Task.OpenDate >= date)
                             .Select(et => new TaskExportDto()
                             {
                                 TaskName = et.Task.Name,
                                 OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                                 DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                                 LabelType = et.Task.LabelType.ToString(),
                                 ExecutionType = et.Task.ExecutionType.ToString()
                             })
                             .OrderByDescending(t => DateTime.ParseExact(t.DueDate, @"d", CultureInfo.InvariantCulture))
                             .ThenBy(t => t.TaskName)
                             .ToList()
                })
                .Take(10)
                .ToList();

            var outputJson = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return outputJson;
        }
    }
}