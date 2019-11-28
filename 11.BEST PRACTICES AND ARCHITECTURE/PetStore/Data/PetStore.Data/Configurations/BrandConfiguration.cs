namespace PetStore.Data.Configurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> brand)
        {
            brand.HasMany(b => b.Toys)
                   .WithOne(t => t.Brand)
                   .HasForeignKey(t => t.BrandId)
                   .OnDelete(DeleteBehavior.Restrict);

            brand.HasMany(b => b.Foods)
                   .WithOne(t => t.Brand)
                   .HasForeignKey(t => t.BrandId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
