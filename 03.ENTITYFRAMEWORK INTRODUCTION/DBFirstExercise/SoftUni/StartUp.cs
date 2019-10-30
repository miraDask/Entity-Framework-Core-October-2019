namespace SoftUni
{
    using System;
    using System.Linq;
    using System.Text;
    using SoftUni.Data;
    public class StartUp
    {
        public static void Main()
        {
            var context = new SoftUniContext();
            var employeesFullInfo = GetEmployeesFullInformation(context);
            Console.WriteLine(employeesFullInfo);

            var employeesWithSalaryOver50000 = GetEmployeesWithSalaryOver50000(context);
            Console.WriteLine(employeesWithSalaryOver50000);
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
    } 
}
