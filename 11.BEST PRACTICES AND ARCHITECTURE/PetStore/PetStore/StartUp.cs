namespace PetStore
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using PetStore.Data;
    using PetStore.Services.Implementations;

    public class StartUp
    {
        public static void Main()
        {
            var db = new PetStoreDbContext();

            using (db)
            {
                //var userService = new UserService(db);
                //var toyService = new ToyService(db, userService);

                //userService.RegisterUser("Pesho", "pessskata@gmail.com");
                // toyService.SellToy(1, 1);
                //foodService.BuyFromDistributor("Dog food", 1, 1, 0.2, DateTime.Now, 2,2 );

                //var toyService = new ToyService(db);

                //toyService.ByuFromDistributor("Ball", "", 1, 0.2, 1, 1);
                //var breedService = new BreedService(db);
                //breedService.AddBreed("Huski");

            }
        }
    }
}
