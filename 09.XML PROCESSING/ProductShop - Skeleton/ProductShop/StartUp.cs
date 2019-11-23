using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            using (var context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //Problem 01:
                //var inputXml = File.ReadAllText(@"./../../../Datasets/users.xml");
                //Console.WriteLine(ImportUsers(context, inputXml));

                //Problem 02:
                //var inputXml = File.ReadAllText(@"./../../../Datasets/products.xml");
                //Console.WriteLine(ImportProducts(context, inputXml));

                ////Problem 03:
                //var inputXml = File.ReadAllText(@"./../../../Datasets/categories.xml");
                //Console.WriteLine(ImportCategories(context, inputXml));

                //Problem 04:
                //var inputXml = File.ReadAllText(@"./../../../Datasets/categories-products.xml");
                //Console.WriteLine(ImportCategoryProducts(context, inputXml));


                //Problem 05
                //Console.WriteLine(GetProductsInRange(context));

                //Problem 06
                //Console.WriteLine(GetSoldProducts(context));

                ////Problem 07
                //Console.WriteLine(GetCategoriesByProductsCount(context));

                //Problem 08
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        //Problem 01:
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<UserImportDto>), new XmlRootAttribute("Users"));

            List<UserImportDto> userDtos ;

            using (var reader = new StringReader(inputXml))
            {
                userDtos = (List<UserImportDto>)xmlSerializer.Deserialize(reader);
            }

            var users = Mapper.Map<List<User>>(userDtos);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //Problem 02:
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<ProductImportDto>), new XmlRootAttribute("Products"));

            var productDtos = new List<ProductImportDto>();

            using (var reader = new StringReader(inputXml))
            {
                productDtos = (List<ProductImportDto>)xmlSerializer.Deserialize(reader);
            }

            var products = Mapper.Map<List<Product>>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Problem 03:
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = 
                new XmlSerializer(typeof(List<CategoryImportDto>), new XmlRootAttribute("Categories"));

            List<CategoryImportDto> categoryImportDtos;

            using (var reader = new StringReader(inputXml))
            {
                categoryImportDtos = (List<CategoryImportDto>)xmlSerializer.Deserialize(reader);
            }

            categoryImportDtos = categoryImportDtos
                .Where(c => c.Name != null).ToList();

            var categories = Mapper.Map<List<Category>>(categoryImportDtos);
            context.Categories.AddRange(categories);
            context.SaveChanges();
            
            return $"Successfully imported {categories.Count}";
        }

        //Problem 04:
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {

            var xmlSerializer = 
                new XmlSerializer(typeof(List<CategoryProductImportDto>),new XmlRootAttribute("CategoryProducts"));

            var categoriesIds = context.Categories
                .Select(c => c.Id)
                .ToList();

            var productsIds = context.Products
                .Select(p => p.Id)
                .ToList();

            var categoriesProductsDtos = new List<CategoryProductImportDto>();

            using (var reader = new StringReader(inputXml))
            {
                categoriesProductsDtos = (List<CategoryProductImportDto>)xmlSerializer.Deserialize(reader);
            }

            categoriesProductsDtos = categoriesProductsDtos
                .Where(cp => categoriesIds.Contains(cp.CategoryId) 
                          && productsIds.Contains(cp.ProductId))
                .ToList();

            var categoriesProducts = Mapper.Map<List<CategoryProduct>>(categoriesProductsDtos);
            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        //Problem 05:
        public static string GetProductsInRange(ProductShopContext context)
        {

            var sb = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(List<ProductInRangeDto>),
                                new XmlRootAttribute("Products"));

            var productsToXmlExport = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ProductInRangeDto>()
                .ToList();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, productsToXmlExport, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 06:
        public static string GetSoldProducts(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var usersWithSoldProducts = context
               .Users
               .Where(u => u.ProductsSold.Any())
               .Select(u => new UserExportDto
               {
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   SoldProducts = u.ProductsSold
                                   .Select(p => new SoldProductexportDto
                                   {
                                       Name = p.Name,
                                       Price = p.Price,
                                   })
                                   .ToList()
               })
               .OrderBy(u => u.LastName)
               .ThenBy(u => u.FirstName)
               .Take(5)
               .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<UserExportDto>),
                                new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (var writer  = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, usersWithSoldProducts, namespaces);
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 07:
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var categoriesByProductsCount = context
                .Categories
                .Select(c => new CategoryExportDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToList();

            var xmlSerializer = new XmlSerializer(typeof(List<CategoryExportDto>),
                              new XmlRootAttribute("Categories"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, categoriesByProductsCount, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var sb = new StringBuilder();

            var usersWithSoldProducts = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count())
                .Select(u => new UserWithSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductWithCountDto
                    {
                        Count = u.ProductsSold.Count(),
                        Products = u.ProductsSold
                               .Select(p => new SoldProductexportDto
                               {
                                  Name = p.Name,
                                  Price = p.Price
                               })
                               .OrderByDescending(p => p.Price)
                               .ToList()
                    }
                })
                .Take(10)
                .ToList();

            var users = new UsersWithCountExportDto()
            {
                Count = context.Users.Count(p => p.ProductsSold.Any()),
                UserWithSoldProductsDto = usersWithSoldProducts
            };

            var xmlSerializer = new XmlSerializer(typeof(UsersWithCountExportDto),
                                new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, users, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}