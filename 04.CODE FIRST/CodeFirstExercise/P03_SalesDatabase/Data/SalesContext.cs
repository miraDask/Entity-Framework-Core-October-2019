﻿namespace P03_SalesDatabase.Data
{
    
    using Models;
    using Microsoft.EntityFrameworkCore;
    
    public class SalesContext : DbContext
    {

        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Store> Stores { get; set; }
        
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ProductConfiguration(modelBuilder);
            CustomerConfiguration(modelBuilder);
            StoreConfiguration(modelBuilder);
            SaleConfiguration(modelBuilder);
        }

        private void ProductConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity => {
                entity.HasKey(p => p.ProductId);

                entity.Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsRequired()
                    .IsUnicode();
                
                entity.Property(p => p.Quantity)
                    .IsRequired();

                entity.Property(p => p.Price)
                    .IsRequired();

                entity.Property(p => p.Description)
                    .HasMaxLength(250)
                    .HasDefaultValue("No description")
                    .IsRequired()
                    .IsUnicode();
            });
        }

        private void CustomerConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity => {
                entity.HasKey(c => c.CustomerId);

                entity.Property(c => c.Name)
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

                entity.Property(c => c.Email)
                    .HasMaxLength(80)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(c => c.CreditCardNumber)
                   // .HasMaxLength(12)
                    .IsRequired()
                    .IsUnicode(false);
            });
        }

        private void StoreConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>(entity => {
                entity.HasKey(s => s.StoreId);

                entity.Property(s => s.Name)
                    .HasMaxLength(80)
                    .IsRequired()
                    .IsUnicode();
            });
        }
        
        private void SaleConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity => {
                entity.HasKey(s => s.SaleId);

                entity.Property(s => s.Date)
                    .IsRequired()
                    .HasColumnType("DATETIME2")
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(s => s.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(s => s.ProductId);

                entity.HasOne(s => s.Customer)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(s => s.CustomerId);

                entity.HasOne(s => s.Store)
                    .WithMany(s => s.Sales)
                    .HasForeignKey(s => s.StoreId);
            });
        }
    }
}
