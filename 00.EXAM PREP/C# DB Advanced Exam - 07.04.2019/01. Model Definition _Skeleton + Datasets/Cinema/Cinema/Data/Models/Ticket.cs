namespace Cinema.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "100000000000000000000")]
        public decimal Price { get; set; }

        public int ProjectionId { get; set; }

        public Projection Projection { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

    }
}
