namespace PetStore.Services.Implementations
{
    using System;
    using System.Linq;
    using PetStore.Data;
    using PetStore.Data.Models;
  
    public class BreedService : IBreedService
    {
        private PetStoreDbContext db;

        public BreedService(PetStoreDbContext data)
        {
            this.db = data;
        }

        public void AddBreed(string name)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace");

            }

            var breed = new Breed()
            {
                Name = name,

            };

            this.db.Breeds.Add(breed);
            db.SaveChanges();
        }

        public bool Exists(int breedId)
            => this.db.Breeds.Any(b => b.Id == breedId);
             
    }
}
