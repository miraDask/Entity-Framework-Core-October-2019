namespace Cinema.DataProcessor.ImportDto
{
    using Cinema.Data.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    class MovieDto
    {

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Director { get; set; }
    }
}
