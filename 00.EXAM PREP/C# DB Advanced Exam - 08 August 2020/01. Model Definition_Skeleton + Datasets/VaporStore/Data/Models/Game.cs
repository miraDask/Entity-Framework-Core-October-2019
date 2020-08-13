namespace VaporStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int DeveloperId { get; set; }

        public virtual Developer Developer { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
            = new HashSet<Purchase>();
        public ICollection<GameTag> GameTags { get; set; }
            = new HashSet<GameTag>();
    }
}
