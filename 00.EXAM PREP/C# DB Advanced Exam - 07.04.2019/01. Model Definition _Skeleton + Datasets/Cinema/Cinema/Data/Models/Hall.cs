namespace Cinema.Data.Models
{
    using System.Collections.Generic;

    public class Hall
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();
    }
}
