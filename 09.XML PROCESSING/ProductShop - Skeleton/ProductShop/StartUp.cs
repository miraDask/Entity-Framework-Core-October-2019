using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                //Problem 03:
                var inputXml = File.ReadAllText(@"./../../../Datasets/categories.xml");
                Console.WriteLine(ImportCategories(context, inputXml));
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
    }
}