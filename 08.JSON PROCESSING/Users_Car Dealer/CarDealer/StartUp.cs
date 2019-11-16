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
                var suppliersFromJson = File.ReadAllText(@"./../../../Datasets/suppliers.json");
                Console.WriteLine(ImportSuppliers(db, suppliersFromJson));

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
    }
}