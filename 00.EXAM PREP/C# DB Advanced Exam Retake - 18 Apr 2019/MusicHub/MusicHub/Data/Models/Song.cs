namespace MusicHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;

    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime CreatedOn  { get; set; }

        public Genre Genre { get; set; }

        public int? AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public int WriterId { get; set; }

        public virtual Writer Writer { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; }
            = new HashSet<SongPerformer>();
    }
}
