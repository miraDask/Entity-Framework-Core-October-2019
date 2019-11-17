using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var db = new CarDealerContext();

            using (db)
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();

                ////Problem 09:
                //var suppliersFromJson = File.ReadAllText(@"./../../../Datasets/suppliers.json");
                //Console.WriteLine(ImportSuppliers(db, suppliersFromJson));

                ////Problem 10:
                //var partsFromJson = File.ReadAllText(@"./../../../Datasets/parts.json");
                //Console.WriteLine(ImportParts(db, partsFromJson));

                //Problem 11:
                //var carsFromJson = File.ReadAllText(@"./../../../Datasets/cars.json");
                //Console.WriteLine(ImportCars(db, carsFromJson));

                //Problem 12:
                //var carsFromJson = File.ReadAllText(@"./../../../Datasets/customers.json");
                //Console.WriteLine(ImportCustomers(db, carsFromJson));

                //Problem 13:
                //var carsFromJson = File.ReadAllText(@"./../../../Datasets/sales.json");
                //Console.WriteLine(ImportSales(db, carsFromJson));

                //Problem 14:
                //Console.WriteLine(GetOrderedCustomers(db));

                //Problem 15:
                //Console.WriteLine(GetCarsFromMakeToyota(db));

                //Problem 16:
                //Console.WriteLine(GetLocalSuppliers(db));

                //Problem 17:
                //Console.WriteLine(GetCarsWithTheirListOfParts(db));

                //Problem 18:
                Console.WriteLine(GetTotalSalesByCustomer(db));

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

        //Problem 10:
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

        //Problem 11:
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDtos = JsonConvert.DeserializeObject<CarDto[]>(inputJson);
            
            var partsIdInDb = context.Parts.Select(c => c.Id).ToList();
            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDto in carDtos)
            {
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    if (!partsIdInDb.Contains(partId))
                    {
                        continue;
                    }

                    partCars.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    });
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partCars);
            var r = context.SaveChanges();

            return $"Successfully imported {cars.Count}."; ;
        }

        //Problem 12:
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}."; ;
        }

        //Problem 13:
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        //Problem 14:
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenByDescending(c => c.IsYoungDriver == false)
                .Select(c => new { 
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    c.IsYoungDriver
                });

            var customersJson = JsonConvert.SerializeObject(customers, Formatting.Indented);
            
            return customersJson;
        }
        
        //Problem 15:
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Where(c => c.Make == "Toyota")
                .Select(c => new { 
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .ToList();

            var outputJson = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);

            return outputJson;
        }

        //Problem 16:
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                });

            var outputJson = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);

            return outputJson;
        }

        //Problem 17:
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartCars
                        .Select(pc => new {
                            pc.Part.Name,
                            Price = pc.Part.Price.ToString("f2")
                        })
                        .ToArray()
                })
                .ToArray();
                

            var outputJson = JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
            return outputJson;
        }

        //Problem 18:
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .ToList();

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var outputJson = JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            });

            return outputJson;
        }
    }
}