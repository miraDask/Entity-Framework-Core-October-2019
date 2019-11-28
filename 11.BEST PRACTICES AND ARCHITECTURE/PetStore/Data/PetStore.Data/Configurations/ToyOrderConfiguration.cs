namespace PetStore.Data.Configurations
{
    using Models;

    using Microsoft.EntityFrameworkCore;

    public class ToyOrderConfiguration : IEntityTypeConfiguration<ToyOrder>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ToyOrder> toyOrder)
        {
            toyOrder.HasKey(to => new { to.ToyId, to.OrderId });

            toyOrder
                .HasOne(to => to.Toy)
                .WithMany(t => t.ToyOrders)
                .HasForeignKey(to => to.ToyId)
                .OnDelete(DeleteBehavior.Restrict);

            toyOrder
                .HasOne(to => to.Order)
                .WithMany(t => t.ToyOrders)
                .HasForeignKey(to => to.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
