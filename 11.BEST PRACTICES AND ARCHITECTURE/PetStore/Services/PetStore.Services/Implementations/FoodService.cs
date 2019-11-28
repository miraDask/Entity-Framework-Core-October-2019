namespace PetStore.Services.Implementations
{
    using System;

    using Data;
    using Data.Models;
    using Models.Food;
    using System.Linq;

    public class FoodService : IFoodService
    {
        private readonly PetStoreDbContext db;
        private readonly IUserService userService;

        public FoodService(PetStoreDbContext data, IUserService userService)
        {
            this.db = data;
            this.userService = userService;
        }

        public void BuyFromDistributor(string name, double weight, decimal price, double profit, DateTime expirationDate, int brandId, int categoryId)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (profit < 0 || profit > 5)
            {
                throw new ArgumentException("Profit must be higher than 0 and lower than 500%");
            }

            var food = new Food()
            {
                Name = name,
                Weight = weight,
                DistributorPrice = price,
                Price = price + (price *(decimal)profit),
                ExpirationDate = expirationDate,
                BrandId = brandId,
                CategoryId = categoryId
            };

            db.Foods.Add(food);
            db.SaveChanges();
        }

        public void BuyFromDistributor(FoodInputServiceModel model)
        {
            if (String.IsNullOrEmpty(model.Name) || String.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");
            }

            if (model.Profit < 0 || model.Profit > 5)
            {
                throw new ArgumentException("Profit must be higher than 0 and lower than 500%");
            }

            var food = new Food()
            {
                Name = model.Name,
                Weight = model.Weight,
                DistributorPrice = model.DistributorPrice,
                Price = model.RetailPrice,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId
            };

            db.Foods.Add(food);
            db.SaveChanges();
        }

        public bool Exists(int foodId)
            => this.db.Foods.Any(f => f.Id == foodId);

        public void SellFood(int foodId, int userId)
        {
            if (!this.Exists(foodId))
            {
                throw new ArgumentException($"There is no food with id {foodId} in the datebase");

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

            var foodOrder = new FoodOrder()
            {
                FoodId = foodId,
                Order = order
            };

            db.Orders.Add(order);
            db.FoodOrders.Add(foodOrder);
            db.SaveChanges();
        }
    }
}
