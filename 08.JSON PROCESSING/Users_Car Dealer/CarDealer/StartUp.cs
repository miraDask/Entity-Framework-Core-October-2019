﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
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
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();

                ////Problem 09:
                //var suppliersFromJson = File.ReadAllText(@"./../../../Datasets/suppliers.json");
                //Console.WriteLine(ImportSuppliers(db, suppliersFromJson));

                ////Problem 10:
                //var partsFromJson = File.ReadAllText(@"./../../../Datasets/parts.json");
                //Console.WriteLine(ImportParts(db, partsFromJson));

                //Problem 11:
                var carsFromJson = File.ReadAllText(@"./../../../Datasets/cars.json");
                Console.WriteLine(ImportCars(db, carsFromJson));
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
    }
}