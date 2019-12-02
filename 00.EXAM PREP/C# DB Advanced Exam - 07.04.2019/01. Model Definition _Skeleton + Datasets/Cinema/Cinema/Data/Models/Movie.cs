namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Cinema.Data.Models.Enums;

    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Genre Genre { get; set; }

        public TimeSpan Duration { get; set; }

        public double Rating { get; set; }

        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; }
    }
}
