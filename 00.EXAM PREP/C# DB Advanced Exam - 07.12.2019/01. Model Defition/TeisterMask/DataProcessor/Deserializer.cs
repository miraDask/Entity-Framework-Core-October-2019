namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Text;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto;
    using System.IO;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using System.Linq;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(ProjectWithTasksImportDto[]),
                                new XmlRootAttribute("Projects"));


            var projectDtos = (ProjectWithTasksImportDto[])xmlSerializer.Deserialize(new StringReader(xmlString));
            var projects = new List<Project>();

            foreach (var dto in projectDtos)
            {
                if (IsValid(dto))
                {
                    var project = new Project()
                    {
                        Name = dto.Name,
                        OpenDate = DateTime.ParseExact(dto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    };

                    if (!string.IsNullOrEmpty(dto.DueDate) && !string.IsNullOrWhiteSpace(dto.DueDate))
                    {
                        project.DueDate = DateTime.ParseExact(dto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }

                    AddValidTasksToProject(project, dto.Tasks, sb);
                    projects.Add(project);
                    sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var employeeDtos = JsonConvert.DeserializeObject<EpmployeeWithTaskImportDto[]>(jsonString);

            foreach (var dto in employeeDtos)
            {
                if (IsValid(dto))
                {

                    var emploee = new Employee()
                    {
                        Username = dto.Username,
                        Email = dto.Email,
                        Phone = dto.Phone
                    };

                    context.Employees.Add(emploee);

                    var tasksIds = new HashSet<int>(dto.Tasks);

                    AddAllEmploeeTasksWithValidTaskId(context, tasksIds, emploee, sb);

                    context.SaveChanges();
                    sb.AppendLine(string.Format(SuccessfullyImportedEmployee, emploee.Username, emploee.EmployeesTasks.Count));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            return sb.ToString().TrimEnd();
        }

      

        private static void AddAllEmploeeTasksWithValidTaskId(TeisterMaskContext context, HashSet<int> tasksIds, int id)
        {
            throw new NotImplementedException();
        }

        private static void AddValidTasksToProject(Project project, TaskImportDto[] tasks, StringBuilder sb)
        {
            foreach (var taskDto in tasks)
            {

                if (IsValid(taskDto))
                {
                    var taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var taskDueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var openDateIsValid = DateTime.Compare(project.OpenDate, taskOpenDate) <= 0;
                    var taskDueDateIsValid = project.DueDate != null 
                        ?  DateTime.Compare((DateTime)project.DueDate, taskDueDate) >= 0
                        : true;

                    if (!openDateIsValid || !taskDueDateIsValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var task = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)int.Parse(taskDto.ExecutionType),
                        LabelType = (LabelType)int.Parse(taskDto.LabelType),

                    };

                    project.Tasks.Add(task);

                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static void AddAllEmploeeTasksWithValidTaskId(TeisterMaskContext context, HashSet<int> tasksIds, Employee emploee, StringBuilder sb)
        {
            var dbTasksIds = context.Tasks.Select(t => t.Id).ToList();

            foreach (var id in tasksIds)
            {
                if (!dbTasksIds.Contains(id))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var employeeTask = new EmployeeTask()
                {
                    Employee = emploee,
                    TaskId = id
                };

                emploee.EmployeesTasks.Add(employeeTask);
            }
        }
    }
}