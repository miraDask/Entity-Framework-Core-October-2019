namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectWithTasksImportDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string OpenDate { get; set; }

        public string DueDate { get; set; }

        public TaskImportDto[] Tasks { get; set; }
    }
}
