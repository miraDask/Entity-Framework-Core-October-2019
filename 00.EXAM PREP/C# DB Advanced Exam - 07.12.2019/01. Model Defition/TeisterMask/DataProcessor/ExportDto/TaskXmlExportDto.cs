﻿namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Xml.Serialization;
    
    [XmlType("Task")]
    public class TaskXmlExportDto
    {
        public string Name { get; set; }

        public string Label { get; set; }
    }
}
