namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> bet)
        {
            bet.HasKey(b => b.BetId);

            bet.Property(b => b.Amount)
            .IsRequired();

            bet.Property(b => b.Prediction)
            .IsRequired();

            bet.Property(b => b.DateTime)
            .IsRequired();

            bet.HasOne(b => b.User)
            .WithMany(u => u.Bets)
            .HasForeignKey(b => b.UserId);

            bet.HasOne(b => b.Game)
            .WithMany(g => g.Bets)
            .HasForeignKey(b => b.GameId);
        }
    }
}
