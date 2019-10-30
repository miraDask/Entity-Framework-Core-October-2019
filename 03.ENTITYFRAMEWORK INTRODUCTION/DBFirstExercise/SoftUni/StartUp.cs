namespace SoftUni
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using SoftUni.Data;
    using SoftUni.Models;

    public class StartUp
    {
        public static void Main()
        {
            var context = new SoftUniContext();
            //var employeesFullInfo = GetEmployeesFullInformation(context);
            //Console.WriteLine(employeesFullInfo);

            //var employeesWithSalaryOver50000 = GetEmployeesWithSalaryOver50000(context);
            //Console.WriteLine(employeesWithSalaryOver50000);

            //var employeesFromResearchAndDevelopment = GetEmployeesFromResearchAndDevelopment(context);
            //Console.WriteLine(employeesFromResearchAndDevelopment);

            //var updatedAddress = AddNewAddressToEmployee(context);
            //Console.WriteLine(updatedAddress);

            //var employeesWithProjectsInPeriod = GetEmployeesInPeriod(context);
            //Console.WriteLine(employeesWithProjectsInPeriod);

            //var addressesByTown = GetAddressesByTown(context);
            //Console.WriteLine(addressesByTown);

            //var employee147 = GetEmployee147(context);
            //Console.WriteLine(employee147);

            var departmentsWithMoreThan5Employees = GetDepartmentsWithMoreThan5Employees(context);
            Console.WriteLine(departmentsWithMoreThan5Employees);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var stringBuilder = new StringBuilder();

            var employees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                stringBuilder.AppendLine(string.Join(" ", e.FirstName, e.LastName, e.MiddleName, e.JobTitle, $"{e.Salary:f2}"));
            }

            return stringBuilder.ToString().TrimEnd();
        }


        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Department = e.Department.Name,
                    e.Salary
                });

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {

            var newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAddress);

            var employee = context.Employees
                .Where(e => e.LastName == "Nakov")
                .FirstOrDefault();

            employee.Address = newAddress;

            context.SaveChanges();

            var sb = new StringBuilder();

            var employees = context.Employees
                .OrderByDescending(e => e.Address.AddressId)
                .Select(e => new
                {
                    Address = e.Address.AddressText
                })
                .Take(10)
                .ToArray();

            foreach (var emp in employees)
            {
                sb.AppendLine(emp.Address);
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                             .Any(ep => ep.Project.StartDate.Year >= 2001
                                        && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                    Projects = e.EmployeesProjects
                                .Select(ep => new
                                {
                                    ProjectName = ep.Project.Name,
                                    ProjectStartDate = ep.Project.StartDate,
                                    ProjectEndDate = ep.Project.EndDate
                                })
                                .ToList()
                })
                .Take(10)
                .ToList();

            foreach (var emp in employees)
            {

                sb.AppendLine($"{emp.FullName} - Manager: {emp.ManagerName}");

                foreach (var project in emp.Projects)
                {
                    var projectStartDate = project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    var projectEndDate = project.ProjectEndDate == null
                        ? "not finished"
                        : project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    sb.AppendLine($"--{project.ProjectName} - {projectStartDate} - {projectEndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var employees = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    Count = a.Employees.Count
                })
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.AddressText}, {e.TownName} - {e.Count} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FullName = e.FirstName + " " + e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                                .OrderBy(ep => ep.Project.Name)
                                .Select(ep => new
                                {
                                    Name = ep.Project.Name
                                })
                                .ToList()
                })
                .FirstOrDefault();

            sb.AppendLine($"{employee.FullName} - {employee.JobTitle}");

            foreach (var project in employee.Projects)
            {
                sb.AppendLine(project.Name);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    Manager = d.Manager.FirstName + " " + d.Manager.LastName,
                    Employees = d.Employees
                                 .OrderBy(e => e.FirstName)
                                 .ThenBy(e => e.LastName)
                                 .Select(e => new
                                 {
                                     FullName = e.FirstName + " " + e.LastName,
                                     e.JobTitle
                                 })
                                
                })
                .ToList();

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.Name} - {dep.Manager}");

                foreach (var emp in dep.Employees)
                {
                    sb.AppendLine($"{emp.FullName} - {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
