namespace PetStore.Services
{
    using System.Collections.Generic;
    using Models.Brand;

    public interface IBrandService
    {
        // returns Id of created Brand
        int Create(string name);

        IEnumerable<BrandListingServiceModel> SearchByName(string name);

        BrandWithToysServiceModel FindByIdWithToys(int id);
    }
}
