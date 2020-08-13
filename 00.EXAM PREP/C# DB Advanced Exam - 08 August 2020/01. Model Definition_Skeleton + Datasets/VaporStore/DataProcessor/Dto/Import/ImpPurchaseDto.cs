namespace VaporStore.DataProcessor.Dto.Import
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class ImpPurchaseDto
    {
        [Required]
        [XmlAttribute("title")]
        public string Titlle { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [RegularExpression(@"^([A-Z0-9]{4})-([A-Z0-9]{4})-([A-Z0-9]{4})$")]
        public string Key { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        [RegularExpression(@"^(\d{4}) (\d{4}) (\d{4}) (\d{4})$")]
        public string Card { get; set; }
    }
}
