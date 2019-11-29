namespace PetStore
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PetStore.Data;
    using PetStore.Data.Models;
    using PetStore.Data.Models.Enumerations;
    using PetStore.Services.Implementations;

    public class StartUp
    {
        public static void Main()
        {
            var db = new PetStoreDbContext();

            using (db)
            {
                // Seeding Database
                //for (int i = 0; i < 10; i++)
                //{
                //    var breed = new Breed
                //    {
                //        Name = "Breed " + i,
                //    };

                //    db.Breeds.Add(breed);

                //}

                //db.SaveChanges();

                //for (int i = 0; i < 30; i++)
                //{
                //    var category = new Category
                //    {
                //        Name = "Category " + i,
                //        Description = "Category Description " + i
                //    };

                //    db.Categories.Add(category);
                //}

                //db.SaveChanges();

                //for (int i = 0; i < 100; i++)
                //{
                //    var category = db.Categories
                //        .OrderBy(c => Guid.NewGuid())
                //        .FirstOrDefault();

                //    var breed = db.Breeds
                //       .OrderBy(c => Guid.NewGuid())
                //       .FirstOrDefault();

                //    var pet = new Pet
                //    {
                //        DateOfBirth = DateTime.UtcNow.AddDays(-60 + i),
                //        Price = 50 + i,
                //        Gender = (Gender)(i % 2),
                //        Desctiption = "Pet Description " + i,
                //        CategoryId = category.Id,
                //        BreedId = breed.Id
                //    };

                //    db.Pets.Add(pet);
                //    category.Pets.Add(pet);
                //    breed.Pets.Add(pet);
                //}

                //db.SaveChanges();
            }
        }
    }
}
