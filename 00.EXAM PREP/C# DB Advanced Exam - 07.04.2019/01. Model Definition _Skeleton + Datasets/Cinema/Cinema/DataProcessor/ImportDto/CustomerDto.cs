namespace Cinema.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Customer")]
    public class CustomerDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [Range(12, 110)]
        public int Age { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "100000000000000000000")]
        public decimal Balance { get; set; }

        [XmlArray("Tickets")]
        public List<TicketDto> Tickets { get; set; }
    }
}
