namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.HasKey(u => u.UserId);

            user.Property(u => u.Username)
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired();

            user.Property(u => u.Password)
            .HasMaxLength(30)
            .IsUnicode(false)
            .IsRequired();

            user.Property(u => u.Email)
            .HasMaxLength(30)
            .IsUnicode(false)
            .IsRequired();

            user.Property(u => u.Name)
           .HasMaxLength(100)
           .IsUnicode()
           .IsRequired();

            user.Property(u => u.Balance)
            .IsRequired();
        }
    }
}
