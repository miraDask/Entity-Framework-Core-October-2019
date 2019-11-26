namespace PetStore.Data.Configurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FoodOrderConfiguration : IEntityTypeConfiguration<FoodOrder>
    {
        public void Configure(EntityTypeBuilder<FoodOrder> foodOrder)
        {
            foodOrder
                .HasKey(fo => new { fo.FoodId, fo.OrderId });

            foodOrder
                .HasOne(fo => fo.Food)
                .WithMany(f => f.FoodOrders)
                .HasForeignKey(fo => fo.FoodId)
                .OnDelete(DeleteBehavior.Restrict);

            foodOrder
               .HasOne(fo => fo.Order)
               .WithMany(f => f.FoodOrders)
               .HasForeignKey(fo => fo.OrderId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
