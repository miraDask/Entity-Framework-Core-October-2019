namespace Cinema.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class TicketDto
    {
        [Required]
        [Range(typeof(decimal), "0.01", "100000000000000000000")]
        public decimal Price { get; set; }

        [Required]
        public int ProjectionId { get; set; }
    }
}
