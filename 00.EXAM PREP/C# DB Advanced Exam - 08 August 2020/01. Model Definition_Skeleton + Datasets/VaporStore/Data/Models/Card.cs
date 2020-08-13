namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using VaporStore.Data.Models.Enums;

    public class Card
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(19)]
        public string Number { get; set; }

        [Required]
        [MaxLength(3)]
        public string Cvc { get; set; }

        [Required]
        public CardType Type { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
            = new HashSet<Purchase>();
    }
}
