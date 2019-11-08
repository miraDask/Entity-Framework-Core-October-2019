namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> town)
        {
            town.HasKey(t => t.TownId);

            town.Property(t => t.Name)
            .HasMaxLength(30)
            .IsUnicode()
            .IsRequired();

            town.HasOne(t => t.Country)
            .WithMany(c => c.Towns)
            .HasForeignKey(t => t.CountryId);
        }
    }
}
