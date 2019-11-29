namespace PetStore.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Data.Models;
    using Data.Models.Enumerations;
    using PetStore.Services.Models.Pet;

    public class PetService : IPetService
    {
        private const int PetsPageSize = 25;

        private readonly PetStoreDbContext db;
        private readonly IBreedService breedService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;


        public PetService(PetStoreDbContext data, IBreedService breedService, ICategoryService categoryService, IUserService userService)
        {
            this.db = data;
            this.breedService = breedService;
            this.categoryService = categoryService;
            this.userService = userService;
        }

        public IEnumerable<PetListingServiceModel> All(int page = 1)
            => this.db
                .Pets
                .Skip((page - 1) * PetsPageSize)
                .Take(PetsPageSize)
                .Select(p => new PetListingServiceModel
                {
                    Id = p.Id,
                    Gender = ((Gender)p.Gender).ToString(),
                    Description = p.Desctiption,
                    Price = p.Price,
                    Breed = p.Breed.Name,
                    Categoty = p.Category.Name
                })
                .ToList();
                
                  

        public void ByuPet(Gender gender, DateTime dateOfBirth, decimal price, string desctiption, int breedId, int categoryId)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price must be higher than 0.");
            }

            if (!this.breedService.Exists(breedId))
            {
                throw new ArgumentException($"There is no breed with id {breedId} in the database.");
            }

            if (!this.categoryService.Exists(categoryId))
            {
                throw new ArgumentException($"There is no category with id {categoryId} in the database.");
            }

            var pet = new Pet()
            {
                Gender = gender,
                DateOfBirth = dateOfBirth,
                Price = price,
                Desctiption = desctiption,
                BreedId = breedId,
                CategoryId = categoryId
            };

            db.Pets.Add(pet);
            db.SaveChanges();
        }

        public bool Exists(int petId)
            => this.db.Pets.Any(p => p.Id == petId);

        public void SellPet(int petId, int userId)
        {
            if (!this.Exists(petId))
            {
                throw new ArgumentException($"There is no toy pet id {petId} in the datebase");

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

            var pet = this.db.Pets.Find(petId);
            order.Pets.Add(pet);
            pet.Order = order;

            db.Orders.Add(order);
            db.SaveChanges();
        }

        public int TotalPets() => this.db.Pets.Count();
    }
}
