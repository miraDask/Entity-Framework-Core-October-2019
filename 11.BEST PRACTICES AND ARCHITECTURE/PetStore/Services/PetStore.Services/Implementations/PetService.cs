namespace PetStore.Services.Implementations
{
    using System;
    using System.Linq;
    using PetStore.Data;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enumerations;

    public class PetService : IPetService
    {
        private readonly PetStoreDbContext db;
        private readonly IBreedService breedService;
        private readonly ICategoryService categoryService;
        private readonly UserService userService;


        public PetService(PetStoreDbContext data, BreedService breedService, ICategoryService categoryService, UserService userService)
        {
            this.db = data;
            this.breedService = breedService;
            this.categoryService = categoryService;
            this.userService = userService;
        }

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
    }
}
