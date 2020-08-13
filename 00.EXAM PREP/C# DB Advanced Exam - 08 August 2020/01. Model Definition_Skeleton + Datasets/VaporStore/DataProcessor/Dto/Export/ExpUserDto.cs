namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExpUserDto
    {
        [XmlAttribute("username")]
        public string UserName { get; set; }

        [XmlArray("Purchases")]
        public ExpPurchaseDto[] Purchases { get; set; }

        public decimal TotalSpent { get; set; }
    }
}
