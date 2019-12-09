namespace TeisterMask.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        [RegularExpression(@"^([(A-Z0-9]*|[a-z0-9]*)$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string Phone { get; set; }

        public ICollection<EmployeeTask> EmployeesTasks { get; set; } = new HashSet<EmployeeTask>();
    }
}
