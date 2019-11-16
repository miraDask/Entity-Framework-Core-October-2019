namespace ProductShop
{
    using System;
    using System.Linq;
    using System.IO;

    using Data;
    using Models;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            using (context)
            {

                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //Problem 01
                //var usersFromJson = File.ReadAllText(@"../../../Datasets/users.json");
                //Console.WriteLine(ImportUsers(context, usersFromJson));

                //Problem 02
                //var productsFromJson = File.ReadAllText(@"../../../Datasets/products.json");
                //Console.WriteLine(ImportProducts(context, productsFromJson));

                //Problem 03
                //var categoriesFromJson = File.ReadAllText(@"../../../Datasets/categories.json");
                //Console.WriteLine(ImportCategories(context, categoriesFromJson));

                //Problem 04
                //var categoryProductsFromJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
                //Console.WriteLine(ImportCategoryProducts(context, categoryProductsFromJson));

                //Problem 05
                //Console.WriteLine(GetProductsInRange(context));

                //Problem 06
                //Console.WriteLine(GetSoldProducts(context));

                ////Problem 07
                //Console.WriteLine(GetCategoriesByProductsCount(context));

                //Problem 08
                //Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        //Problem 01
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

        //Problem 02
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

        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categoriesFromJson = JsonConvert.DeserializeObject<Category[]>(inputJson);
            var validEntities = categoriesFromJson
                .Where(c => c.Name != null && c.Name.Length >= 3 && c.Name.Length <= 15)
                .ToList();

            context.Categories.AddRange(validEntities);
            context.SaveChanges();

            return $"Successfully imported {validEntities.Count}";
        }

        //Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        //Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .ToList();

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var outputJson = JsonConvert.SerializeObject(products, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = resolver
            });

            return outputJson;
        }

        //Problem 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersWithSoldProducts = context
                .Users
                .Where(u => u.ProductsSold.Any(sp => sp.Buyer != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                                    .Where(p => p.Buyer != null)
                                    .Select(p => new
                                    {
                                        p.Name,
                                        p.Price,
                                        BuyerFirstName = p.Buyer.FirstName,
                                        BuyerLastName = p.Buyer.LastName
                                    })
                                    .ToList()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };

            var outputJson = JsonConvert.SerializeObject(usersWithSoldProducts, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,

            });

            return outputJson;
        }

        //Problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoriesByProductsCount = context
                .Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2"),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price).ToString("f2")
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToList();

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var outputJson = JsonConvert.SerializeObject(categoriesByProductsCount, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            });

            return outputJson;
        }

        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithSoldProducts = context
                .Users
                .Where(u => u.ProductsSold.Any(sp => sp.Buyer != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold
                                        .Where(p => p.Buyer != null)
                                        .Count(),
                        Products = u.ProductsSold
                               .Where(p => p.Buyer != null)
                               .Select(p => new
                               {
                                   p.Name,
                                   p.Price
                               })
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var users = new
            {
                UsersCount = usersWithSoldProducts.Count,
                Users = usersWithSoldProducts,
            };

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };

            var outputJson = JsonConvert.SerializeObject(users, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore

            });

            return outputJson;
        }
    }
}