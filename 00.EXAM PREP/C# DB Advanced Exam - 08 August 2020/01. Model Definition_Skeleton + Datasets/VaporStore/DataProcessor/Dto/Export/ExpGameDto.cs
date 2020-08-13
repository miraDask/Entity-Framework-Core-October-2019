namespace VaporStore.DataProcessor.Dto.Export
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Game")]
    public class ExpGameDto
    {
        [XmlAttribute("title")]
        public string Titile { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }
    }
}
