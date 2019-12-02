namespace Cinema.Data
{
    using Cinema.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CinemaContext : DbContext
    {
        public CinemaContext()  { }

        public CinemaContext(DbContextOptions options)
            : base(options)   { }


        public DbSet<Movie> Movies { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<Projection> Projections { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);


            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hall>(hall => 
            {
                hall.HasKey(h => h.Id);

                hall.HasMany(h => h.Seats)
                    .WithOne(s => s.Hall)
                    .HasForeignKey(s => s.HallId);

                hall.HasMany(h => h.Projections)
                   .WithOne(s => s.Hall)
                   .HasForeignKey(s => s.HallId);

            });

            modelBuilder.Entity<Projection>(projection => 
            {
                projection.HasKey(p => p.Id);

                projection.HasMany(p => p.Tickets)
                          .WithOne(t => t.Projection)
                          .HasForeignKey(t => t.ProjectionId);
            });

            modelBuilder.Entity<Movie>(movie =>
            {
                movie.HasKey(m => m.Id);

                movie.HasMany(m => m.Projections)
                          .WithOne(p => p.Movie)
                          .HasForeignKey(p => p.MovieId);
            });

            modelBuilder.Entity<Customer>(customer =>
            {
                customer.HasKey(c => c.Id);

                customer.HasMany(c => c.Tickets)
                          .WithOne(t => t.Customer)
                          .HasForeignKey(t => t.CustomerId);
            });
        }
    }
}