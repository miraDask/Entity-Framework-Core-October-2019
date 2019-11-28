namespace PetStore.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using PetStore.Data;
    using Models.Food;
    using PetStore.Data.Models;

    public class FoodService : IFoodService
    {
        private readonly PetStoreDbContext db;

        public FoodService(PetStoreDbContext data) => this.db = data;
        

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
    }
}
