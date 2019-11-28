using PetStore.Data;
using PetStore.Data.Models.Enumerations;
using System;

namespace PetStore.Services.Implementations
{
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
