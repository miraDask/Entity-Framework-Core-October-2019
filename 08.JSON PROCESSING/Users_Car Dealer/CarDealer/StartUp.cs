using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var db = new CarDealerContext();

            using (db)
            {
                //db.Database.EnsureCreated();

                //Problem 09:
                //var suppliersFromJson = File.ReadAllText(@"./../../../Datasets/suppliers.json");
                //Console.WriteLine(ImportSuppliers(db, suppliersFromJson));

                var partsFromJson = File.ReadAllText(@"./../../../Datasets/parts.json");
                Console.WriteLine(ImportParts(db, partsFromJson));

            }
        }
        
        //Problem 09:
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}."; ;
        }

        //Problem 09:
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);

            var validSellersIds = context.Suppliers
                .Select(s => s.Id)
                .ToList();

            var validEntities = parts
                .Where(p => validSellersIds.Contains(p.SupplierId))
                .ToList();


            context.Parts.AddRange(validEntities);
            context.SaveChanges();

            return $"Successfully imported {validEntities.Count}."; ;
        }
    }
}