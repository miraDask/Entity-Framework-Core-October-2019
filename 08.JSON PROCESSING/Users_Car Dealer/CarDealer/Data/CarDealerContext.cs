using CarDealer.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        public CarDealerContext(DbContextOptions options)
            : base(options)
        {
        }

        public CarDealerContext()
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Part> Parts { get; set; }
        
        public DbSet<PartCar> PartCars { get; set; }
        
        public DbSet<Sale> Sales { get; set; }
        
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-2HCAHJD\SQLEXPRESS;Database=CarDealer;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>(e =>
            {
                e.HasKey(k => new { k.CarId, k.PartId });
            });

            modelBuilder.Entity<Car>(car =>
            {
                car.HasMany(c => c.PartCars)
                    .WithOne(pc => pc.Car)
                    .HasForeignKey(pc => pc.CarId);

                car.HasMany(c => c.Sales)
                    .WithOne(s => s.Car)
                    .HasForeignKey(s => s.CarId);
            });

            modelBuilder.Entity<Part>(part =>
            {
                part.HasMany(p => p.PartCars)
                    .WithOne(pc => pc.Part)
                    .HasForeignKey(pc => pc.PartId);

                part.HasOne(p => p.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(p => p.SupplierId);
            });

            modelBuilder.Entity<Customer>(cust =>
            {
                cust.HasMany(c => c.Sales)
                    .WithOne(s => s.Customer)
                    .HasForeignKey(s => s.CustomerId);

            });
        }
    }
}
