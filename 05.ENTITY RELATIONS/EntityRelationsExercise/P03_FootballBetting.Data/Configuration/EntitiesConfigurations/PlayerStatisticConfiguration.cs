namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> entity)
        {
            entity.HasKey(ps => new { ps.GameId, ps.PlayerId });

            entity.Property(ps => ps.ScoredGoals)
            .IsRequired();

            entity.Property(ps => ps.Assists)
            .IsRequired();

            entity.Property(ps => ps.MinutesPlayed)
            .IsRequired();

            entity.HasOne(ps => ps.Player)
            .WithMany(p => p.PlayerStatistics)
            .HasForeignKey(ps => ps.PlayerId);

            entity.HasOne(ps => ps.Game)
            .WithMany(p => p.PlayerStatistics)
            .HasForeignKey(ps => ps.GameId);
        }
    }
}
