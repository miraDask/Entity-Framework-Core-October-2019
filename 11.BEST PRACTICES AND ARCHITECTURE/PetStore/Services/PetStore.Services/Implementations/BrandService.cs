namespace PetStore.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    using Data;
    using Data.Models;
    using Data.Models.Validations;
    using Models.Brand;
    using Models.Toy;

    public class BrandService : IBrandService
    {
        private readonly PetStoreDbContext db;

        public BrandService(PetStoreDbContext data) 
            => this.db = data;
        
        public int Create(string name)
        {
            if (name.Length > DataValidations.NameMaxLength)
            {
                throw new InvalidOperationException($"Brand name cannot be more than {DataValidations.NameMaxLength} characters");
            }

            if (db.Brands.Any(b => b.Name == name))
            {
                throw new InvalidOperationException($"Brand name {name} already exists");

            }

            var brand = new Brand() { Name = name };
            db.Brands.Add(brand);
            db.SaveChanges();

            return brand.Id;
        }

        public BrandWithToysServiceModel FindByIdWithToys(int id)
            => this.db
                   .Brands
                   .Where(b => b.Id == id)
                   .Select(b => new BrandWithToysServiceModel
                   {
                       Name = b.Name,
                       Toys = b.Toys
                                .Select(t => new ToyListingServiceModel {
                                    Id = t.Id,
                                    Name = t.Name,
                                    Price = t.Price,
                                    TotalOrders = t.ToyOrders.Count
                                })
                                .ToList()
                   
                   })
                   .FirstOrDefault();


        public IEnumerable<BrandListingServiceModel> SearchByName(string name)
            => this.db
                   .Brands
                   .Where(b => b.Name.ToLower().Contains(name.ToLower()))
                   .Select(b => new BrandListingServiceModel
                   {
                       Id = b.Id,
                       Name = b.Name
                   })
                   .ToList();

    }
}
