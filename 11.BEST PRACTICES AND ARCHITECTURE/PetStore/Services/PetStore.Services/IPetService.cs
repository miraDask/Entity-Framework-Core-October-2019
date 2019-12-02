namespace PetStore.Services
{
    using System;
    using System.Collections.Generic;
    using Data.Models.Enumerations;
    using PetStore.Services.Models.Pet;

    public interface IPetService
    {
        IEnumerable<PetListingServiceModel> All(int page = 1);

        void ByuPet(Gender gender, DateTime dateOfBirth, decimal price, string desctiption, int breedId, int categoryId);
        
        void SellPet(int petId, int userId);

        bool Exists(int petId);

        int TotalPets();

        PetListingServiceModel PetDetails(int id);

        bool DeletePet(int id);
    }
}
