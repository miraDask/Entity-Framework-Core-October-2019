namespace PetStore.Services.Implementations
{
    using System;
    using PetStore.Data;
    using PetStore.Data.Models;
    using PetStore.Services.Models.Toy;

    public class ToyService : IToyService
    {
        private readonly PetStoreDbContext db;

        public ToyService(PetStoreDbContext data) => this.db = data;

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
    }
}
