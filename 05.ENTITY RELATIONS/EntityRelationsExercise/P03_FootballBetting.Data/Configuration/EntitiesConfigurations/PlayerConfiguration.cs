namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> player)
        {
            player.HasKey(p => p.PlayerId);

            player.Property(p => p.Name)
            .HasMaxLength(30)
            .IsUnicode()
            .IsRequired();

            player.Property(p => p.SquadNumber)
            .IsRequired();

            player.Property(p => p.IsInjured)
            .IsRequired();

            player.HasOne(p => p.Position)
            .WithMany(p => p.Players)
            .HasForeignKey(p => p.PositionId);

            player.HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId);
        }
    }
}
