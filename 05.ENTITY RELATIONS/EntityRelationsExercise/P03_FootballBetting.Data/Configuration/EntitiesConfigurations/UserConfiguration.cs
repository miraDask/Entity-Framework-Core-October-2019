namespace P03_FootballBetting.Data.Configuration.EntitiesConfigurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static DataValidations.User;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.HasKey(u => u.UserId);

            user.Property(u => u.Username)
            .HasMaxLength(UsernameMaxLength)
            .IsUnicode(false)
            .IsRequired();

            user.Property(u => u.Password)
            .HasMaxLength(PasswordMaxLength)
            .IsUnicode(false)
            .IsRequired();

            user.Property(u => u.Email)
            .HasMaxLength(EmailMaxLength)
            .IsUnicode(false)
            .IsRequired();

            user.Property(u => u.Name)
           .HasMaxLength(NameMaxLength)
           .IsUnicode()
           .IsRequired();

            user.Property(u => u.Balance)
            .IsRequired();
        }
    }
}
