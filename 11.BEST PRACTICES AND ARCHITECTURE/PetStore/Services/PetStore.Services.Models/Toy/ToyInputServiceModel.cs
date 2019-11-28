namespace PetStore.Services.Models.Toy
{
    public class ToyInputServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Profit { get; set; }

        public decimal DistributorPrice { get; set; }

        public decimal RetailPrice => DistributorPrice + (DistributorPrice * (decimal)Profit);

        public int BrandId { get; set; }

        public int CategoryId { get; set; }
    }
}
