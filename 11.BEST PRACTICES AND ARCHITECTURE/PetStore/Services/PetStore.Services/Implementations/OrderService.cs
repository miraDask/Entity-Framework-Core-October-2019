namespace PetStore.Services.Implementations
{
    using System;

    using Data;
    using Data.Models.Enumerations;

    public class OrderService : IOrderService
    {
        private readonly PetStoreDbContext db;
        public OrderService(PetStoreDbContext data) => this.db = data;

        public void CompleteOrder(int orderId)
        {
            var order = db.Orders.Find(orderId);

            if (order == null)
            {
                throw new InvalidOperationException($"There is no order with id {orderId} in database.");
            }

            order.Status = OrderStatus.Done;
            db.SaveChanges();
        }
    }
}
