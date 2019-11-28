namespace PetStore.Data.Configurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> user)
        {
            user.Property(u => u.Email)
                .IsUnicode(false);

            user.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
