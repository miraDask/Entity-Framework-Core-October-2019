﻿using System;
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

                var suppliersFromJson = File.ReadAllText(@"./../../../Datasets/suppliers.json");
                var partsFromJson = File.ReadAllText(@"./../../../Datasets/parts.json");
                var carsFromJson = File.ReadAllText(@"./../../../Datasets/cars.json");
                var customersFromJson = File.ReadAllText(@"./../../../Datasets/customers.json");
                var salesFromJson = File.ReadAllText(@"./../../../Datasets/sales.json");

                Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

                ////Problem 09:
                //Console.WriteLine(ImportSuppliers(db, suppliersFromJson));

                ////Problem 10:
                //Console.WriteLine(ImportParts(db, partsFromJson));

                //Problem 11:
                //Console.WriteLine(ImportCars(db, carsFromJson));

                //Problem 12:
                //Console.WriteLine(ImportCustomers(db, customersFromJson));

                //Problem 13:
                //Console.WriteLine(ImportSales(db, salesFromJson));

                //Problem 14:
                //Console.WriteLine(GetOrderedCustomers(db));

                //Problem 15:
                //Console.WriteLine(GetCarsFromMakeToyota(db));

                //Problem 16:
                Console.WriteLine(GetLocalSuppliers(db));

                //Problem 17:
                //Console.WriteLine(GetCarsWithTheirListOfParts(db));

                //Problem 18:
                //Console.WriteLine(GetTotalSalesByCustomer(db));

                //Problem 19:
                //Console.WriteLine(GetSalesWithAppliedDiscount(db));

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
            var carDtos = JsonConvert.DeserializeObject<CarFromJsonDto[]>(inputJson);

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
                .ToList();

            var customersMaped = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(customers);

            var customersToJson = JsonConvert.SerializeObject(customersMaped, Formatting.Indented);

            return customersToJson;
        }

        //Problem 15:
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Where(c => c.Make == "Toyota")
                .ToList();

            var carsDto = Mapper.Map<IEnumerable<Car>, IEnumerable<CarToJsonDto>>(toyotaCars);

            var outputJson = JsonConvert.SerializeObject(carsDto, Formatting.Indented);

            return outputJson;
        }

        //Problem 16:
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ToList();

            var localSuppliersDtos = Mapper.Map<IEnumerable<Supplier>, IEnumerable<LocalSupplierDto>>(localSuppliers);

            var outputJson = JsonConvert.SerializeObject(localSuppliersDtos, Formatting.Indented);

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
                        .Select(pc => new
                        {
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

        //Problem 19:
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var firstTenSales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = s.Discount.ToString("f2"),
                    price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    priceWithDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price)
                        - s.Car.PartCars.Sum(pc => pc.Part.Price)
                        * (s.Discount / 100m))
                        .ToString("f2")
                })
                .ToList();

            var outputJson = JsonConvert.SerializeObject(firstTenSales, Formatting.Indented);

            return outputJson;
        }
    }
}