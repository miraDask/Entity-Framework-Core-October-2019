namespace PetStore.Services.Implementations
{
    using System;
    using System.Linq;

    using Data;
    using Data.Models;
    using Models.Toy;

    public class ToyService : IToyService
    {
        private readonly PetStoreDbContext db;
        private readonly UserService userService;

        public ToyService(PetStoreDbContext data, UserService userService)
        {
            this.db = data;
            this.userService = userService;
        } 

        public void ByuFromDistributor(string name, string description, decimal price, double profit, int brandId, int categoryId)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (profit < 0 || profit > 5)
            {
                throw new ArgumentException("Profit must be higher than 0 and lower than 500%");
            }

            var toy = new Toy()
            {
                Name = name,
                Description = description,
                DistributorPrice = price,
                Price = price + (price * (decimal)profit),
                BrandId = brandId,
                CategoryId = categoryId
            };

            db.Toys.Add(toy);
            db.SaveChanges();
        }

        public void ByuFromDistributor(ToyInputServiceModel model)
        {
            if (String.IsNullOrEmpty(model.Name) || String.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (model.Profit < 0 || model.Profit > 5)
            {
                throw new ArgumentException("Profit must be higher than 0 and lower than 500%");
            }

            var toy = new Toy()
            {
                Name = model.Name,
                Description = model.Description,
                DistributorPrice = model.DistributorPrice,
                Price = model.RetailPrice,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

            db.Toys.Add(toy);
            db.SaveChanges();
        }

        public bool Exists(int toyId)
            => this.db.Toys.Any(t => t.Id == toyId);

        public void SellToy(int toyId, int userId)
        {
            if (!this.Exists(toyId))
            {
                throw new ArgumentException($"There is no toy with id {toyId} in the datebase");

            }

            if (!this.userService.Exists(userId))
            {
                throw new ArgumentException($"There is no user with id {userId} in the datebase");
            }

            var order = new Order()
            {
                PurchaseDate = DateTime.Now,
                Status = Data.Models.Enumerations.OrderStatus.Done,
                UserId = userId
            };

            var toyOrder = new ToyOrder()
            {
                ToyId = toyId,
                Order = order
            };

            db.Orders.Add(order);
            db.ToyOrders.Add(toyOrder);
            db.SaveChanges();
        }
    }
}
