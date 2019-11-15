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

            var usersFromJson = File.ReadAllText(@"../../../Datasets/users.json");

            Console.WriteLine(ImportUsers(context, usersFromJson));
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
    }
}