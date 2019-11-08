namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> team)
        {
            team.HasKey(t => t.TeamId);

            team.Property(t => t.Name)
            .HasMaxLength(50)
            .IsUnicode()
            .IsRequired();

            team.Property(t => t.LogoUrl)
            .HasMaxLength(250)
            .IsUnicode(false)
            .IsRequired(false);

            team.Property(t => t.Initials)
            .HasMaxLength(3)
            .IsUnicode()
            .IsRequired();

            team.Property(t => t.Budget)
            .IsRequired();

            team.HasOne(t => t.PrimaryKitColor)
            .WithMany(c => c.PrimaryKitTeams)
            .HasForeignKey(t => t.PrimaryKitColorId)
            .OnDelete(DeleteBehavior.Restrict);

            team.HasOne(t => t.SecondaryKitColor)
            .WithMany(c => c.SecondaryKitTeams)
            .HasForeignKey(t => t.SecondaryKitColorId)
            .OnDelete(DeleteBehavior.Restrict);

            team.HasOne(t => t.Town)
            .WithMany(t => t.Teams)
            .HasForeignKey(t => t.TownId);
        }
    }
}
