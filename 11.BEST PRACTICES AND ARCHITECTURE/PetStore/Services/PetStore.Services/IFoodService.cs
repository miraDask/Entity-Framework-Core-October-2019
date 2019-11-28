namespace PetStore.Services
{
    using System;

    using Models.Food;
   
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

        void SellFood(int foodId, int userId);

        bool Exists(int foodId);
    }
}
