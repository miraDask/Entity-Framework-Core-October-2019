namespace P01_StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Data.Models;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using static DataValidator.Resource;

    public class ResourceEntityConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> resource)
        {
            resource.HasKey(r => r.ResourceId);

            resource.Property(r => r.Name)
                .HasMaxLength(NameMaxLength)
                .IsUnicode()
                .IsRequired();

            resource.Property(r => r.Url)
                .IsUnicode(false)
                .IsRequired();

        }
    }
}
