namespace ProductShop
{
    using System;
    using System.Linq;
    using System.IO;

    using Data;
    using Models;
    
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var usersFromJson = File.ReadAllText(@"../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usersFromJson));

            var productsFromJson = File.ReadAllText(@"../../../Datasets/products.json");
            Console.WriteLine(ImportProducts(context, productsFromJson));

        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            var validEntities = users
                .Where(u => u.LastName != null && u.LastName.Length >= 3)
                .ToList();

            context.Users.AddRange(validEntities);
            context.SaveChanges();

            return $"Successfully imported {validEntities.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var produstsFromJson = JsonConvert.DeserializeObject<Product[]>(inputJson);
            var validEntities = produstsFromJson
                .Where(p => p.Name != null && p.Name.Length >= 3)
                .ToList();

            context.Products.AddRange(validEntities);
            context.SaveChanges();

            return $"Successfully imported {validEntities.Count}";
        }
    }
}