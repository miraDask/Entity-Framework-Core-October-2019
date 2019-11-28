namespace PetStore.Services
{
    using System;

    using Data.Models.Enumerations;

    public interface IPetService
    {
        void ByuPet(Gender gender, DateTime dateOfBirth, decimal price, string desctiption, int breedId, int categoryId);
        
        void SellPet(int petId, int userId);

        bool Exists(int petId);
    }
}
