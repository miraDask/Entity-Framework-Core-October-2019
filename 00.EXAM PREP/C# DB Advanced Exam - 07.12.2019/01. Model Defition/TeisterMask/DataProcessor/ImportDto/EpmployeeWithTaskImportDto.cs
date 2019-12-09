namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EpmployeeWithTaskImportDto
    {

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

        public List<int> Tasks { get; set; }
    }
}
