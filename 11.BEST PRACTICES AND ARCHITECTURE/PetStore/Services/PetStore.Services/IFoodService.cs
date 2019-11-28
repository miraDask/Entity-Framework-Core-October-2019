namespace PetStore.Services
{
    using PetStore.Services.Models.Food;
    using System;
   
    public interface IFoodService
    {
        void BuyFromDistributor
            (
                string name,
                double weight,
                decimal price,
                double profit,
                DateTime expirationDate,
                int brandId,
                int categoryId
            );

        void BuyFromDistributor(FoodInputServiceModel food);
    }
}
