namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> position)
        {
            position.HasKey(p => p.PositionId);

            position.Property(p => p.Name)
            .HasMaxLength(20)
            .IsUnicode()
            .IsRequired();
        }
    }
}
