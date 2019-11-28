namespace PetStore
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using PetStore.Data;
    using PetStore.Data.Models.Enumerations;
    using PetStore.Services.Implementations;

    public class StartUp
    {
        public static void Main()
        {
            var db = new PetStoreDbContext();

            using (db)
            {
                //var toyService = new ToyService(db, userService);

                //userService.RegisterUser("Pesho", "pessskata@gmail.com");
                // toyService.SellToy(1, 1);
                //foodService.BuyFromDistributor("Dog food", 1, 1, 0.2, DateTime.Now, 2,2 );

                //var toyService = new ToyService(db);

                ////toyService.ByuFromDistributor("Ball", "", 1, 0.2, 1, 1);
                ////breedService.AddBreed("Huski");
                //var userService = new UserService(db);
                //var categoryService = new CategoryService(db);
                //var breedService = new BreedService(db);
                //var petService = new PetService(db,breedService, categoryService, userService);

                ////petService.ByuPet(Gender.Male, DateTime.Now, 1, "very small dog", 1, 2);
                //petService.SellPet(1, 1);

            }
        }
    }
}
