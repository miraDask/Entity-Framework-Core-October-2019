namespace P03_FootballBetting.Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DataSettings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.TeamId);

                entity.Property(t => t.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

                entity.Property(t => t.LogoUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .IsRequired(false);

                entity.Property(t => t.Initials)
                .HasMaxLength(3)
                .IsUnicode()
                .IsRequired();

                entity.Property(t => t.Budget)
                .IsRequired();

                entity.HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.TownId);
            });

            builder.Entity<Color>(entity =>
            {
                entity.HasKey(c => c.ColorId);

                entity.Property(c => c.Name)
                .HasMaxLength(30)
                .IsUnicode()
                .IsRequired();

            });

            builder.Entity<Town>(entity => {
                entity.HasKey(t => t.TownId);

                entity.Property(t => t.Name)
                .HasMaxLength(30)
                .IsUnicode()
                .IsRequired();

                entity.HasOne(t => t.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(t => t.CountryId);
            });

            builder.Entity<Country>(entity => {
                entity.HasKey(c => c.CountryId);

                entity.Property(c => c.Name)
                .HasMaxLength(30)
                .IsUnicode()
                .IsRequired();

            });

            builder.Entity<Player>(entity => {
                entity.HasKey(p => p.PlayerId);

                entity.Property(p => p.Name)
                .HasMaxLength(30)
                .IsUnicode()
                .IsRequired();

                entity.Property(p => p.SquadNumber)
                .IsRequired();

                entity.Property(p => p.IsInjured)
                .IsRequired();

                entity.HasOne(p => p.Position)
                .WithMany(p => p.Players)
                .HasForeignKey(p => p.PositionId);

                entity.HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

            });

            builder.Entity<Position>(entity => {
                entity.HasKey(p => p.PositionId);

                entity.Property(p => p.Name)
                .HasMaxLength(20)
                .IsUnicode()
                .IsRequired();
            });

            builder.Entity<PlayerStatistic>(entity => {
                entity.HasKey(ps => new { ps.GameId, ps.PlayerId});

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
            });

            builder.Entity<Game>(entity => {
                entity.HasKey(g => g.GameId);

                entity.Property(g => g.HomeTeamGoals)
                .IsRequired();

                entity.Property(g => g.AwayTeamGoals)
                .IsRequired();

                entity.Property(g => g.DateTime)
                .HasColumnType("DATETIME2")
                .IsRequired();

                entity.Property(g => g.HomeTeamBetRate)
                .IsRequired();

                entity.Property(g => g.AwayTeamBetRate)
                .IsRequired();

                entity.Property(g => g.DrawBetRate)
                .IsRequired();

                entity.Property(g => g.Result)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsRequired();

                entity.HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Bet>(entity => {
                entity.HasKey(b => b.BetId);

                entity.Property(b => b.Amount)
                .IsRequired();

                entity.Property(b => b.Prediction)
                .IsRequired();

                entity.Property(b => b.DateTime)
                .IsRequired();

                entity.HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId);

                entity.HasOne(b => b.Game)
                .WithMany(g => g.Bets)
                .HasForeignKey(b => b.GameId);
            });

            builder.Entity<User>(entity => {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

                entity.Property(u => u.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

                entity.Property(u => u.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsRequired();

                entity.Property(u => u.Name)
               .HasMaxLength(100)
               .IsUnicode()
               .IsRequired();

                entity.Property(u => u.Balance)
                .IsRequired();
            });
        }
    }
}
