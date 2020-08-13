namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class ExpPurchaseDto
    {
        public string Card { get; set; }

        public string Cvc { get; set; }

        public string Date { get; set; }

        public ExpGameDto Game { get; set; }
    }
}
