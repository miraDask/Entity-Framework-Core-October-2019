namespace PetStore.Services
{

    using Models.Toy;
    
    public interface IToyService
    {
        void ByuFromDistributor(string name, string description, decimal price, double profit, int brandId, int categoryId);

        void ByuFromDistributor(ToyInputServiceModel model);
    }
}
