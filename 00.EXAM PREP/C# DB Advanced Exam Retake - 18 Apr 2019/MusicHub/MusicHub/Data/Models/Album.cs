namespace MusicHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal Price
        => this.Songs.Sum(p => p.Price);

        public int? ProducerId { get; set; }

        public virtual Producer Producer { get; set; }

        public ICollection<Song> Songs { get; set; }
            = new HashSet<Song>();
    }
}
