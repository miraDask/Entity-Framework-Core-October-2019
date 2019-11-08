namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> country)
        {
            country.HasKey(c => c.CountryId);

            country.Property(c => c.Name)
            .HasMaxLength(30)
            .IsUnicode()
            .IsRequired();
        }
    }
}
