namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using TeisterMask.Data.Models.Enums;

    [XmlType("Task")]
    public class TaskImportDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string OpenDate { get; set; }

        [Required]
        public string DueDate { get; set; }

        [Required]
        public string ExecutionType { get; set; }

        [Required]
        public string LabelType { get; set; }
    }
}
