namespace PetStore.Services.Models.Food
{
    using System;

     public class FoodInputServiceModel
    {
        public string Name { get; set; }

        public double Weight { get; set; }

        public DateTime ExpirationDate { get; set; }

        public double Profit { get; set; }

        public decimal DistributorPrice { get; set; }

        public decimal RetailPrice => DistributorPrice + (DistributorPrice * (decimal)Profit);

        public int BrandId { get; set; }

        public int CategoryId { get; set; }
    }
}
