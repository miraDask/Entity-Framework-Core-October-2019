namespace P01_HospitalDatabase.Data
{
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;

    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(p => p.Email)
                      .IsUnicode(false);

                entity.HasMany(p => p.Visitations)
                      .WithOne(v => v.Patient);

                entity.HasMany(p => p.Diagnoses).WithOne(d => d.Patient);
            });


            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(e => new
                {
                    e.PatientId,
                    e.MedicamentId
                });

                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Prescriptions);

                entity.HasOne(e => e.Medicament)
                      .WithMany(p => p.Prescriptions);
            });
        }
    }
}
