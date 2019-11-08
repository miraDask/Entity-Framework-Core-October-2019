namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> color)
        {
            color.HasKey(c => c.ColorId);

            color.Property(c => c.Name)
            .HasMaxLength(30)
            .IsUnicode()
            .IsRequired();
        }
    }
}
