namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectWithTasksExportDto
    {
        [XmlAttribute]
        public string TasksCount { get; set; }

        [XmlElement]
        public string ProjectName { get; set; }

        [XmlElement]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskXmlExportDto[] Tasks { get; set; }
    }
}
