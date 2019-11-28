namespace PetStore
{
    using Microsoft.EntityFrameworkCore;
    using PetStore.Data;
    using PetStore.Services.Implementations;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            var db = new PetStoreDbContext();

            using (db)
            {
                var foodService = new FoodService(db);

                foodService.BuyFromDistributor("Dog food", 1, 1, 0.2, DateTime.Now, 2,2 );
            }
        }
    }
}
