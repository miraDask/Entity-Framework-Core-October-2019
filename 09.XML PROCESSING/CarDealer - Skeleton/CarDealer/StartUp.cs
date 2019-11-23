using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            using (context)
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //Problem 09:
                //var inputXml = File.ReadAllText("./../../../Datasets/suppliers.xml");
                //Console.WriteLine(ImportSuppliers(context, inputXml));

                //Problem 10:
                //var inputXml = File.ReadAllText("./../../../Datasets/parts.xml");
                //Console.WriteLine(ImportParts(context, inputXml));

                ////Problem 11:
                //var inputXml = File.ReadAllText("./../../../Datasets/cars.xml");
                //Console.WriteLine(ImportCars(context, inputXml));

                //Problem 12:
                var inputXml = File.ReadAllText("./../../../Datasets/customers.xml");
                Console.WriteLine(ImportCustomers(context, inputXml));

            }
        }

        //Problem 09:
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<SupplierImportDto>),
                                new XmlRootAttribute("Suppliers"));

            List<SupplierImportDto> suppliersDtos;

            using (var reader = new StringReader(inputXml))
            {
                suppliersDtos = (List<SupplierImportDto>)xmlSerializer.Deserialize(reader);
            }

            var suppliers = Mapper.Map<List<SupplierImportDto>, List<Supplier>>(suppliersDtos);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }
        
        //Problem 10:
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<PartImportDto>),
                                new XmlRootAttribute("Parts"));

            var suppliersIds = context.Suppliers.Select(p => p.Id);

            List<PartImportDto> partsDtos;

            using (var reader = new StringReader(inputXml))
            {
                partsDtos = (List<PartImportDto>)xmlSerializer.Deserialize(reader);
            }

            var parts = Mapper.Map<List<PartImportDto>, List<Part>>(partsDtos);
            parts = parts
                .Where(p => suppliersIds.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //Problem 11:
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<CarImportDto>),
                                new XmlRootAttribute("Cars"));

            List<CarImportDto> carDtos;

            using (var reader = new StringReader(inputXml))
            {
                carDtos = (List<CarImportDto>)xmlSerializer.Deserialize(reader);
            }

            var partsIdInDb = context.Parts.Select(c => c.Id).ToList();
            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDto in carDtos)
            {
                var car = Mapper.Map<Car>(carDto);

                var parts = carDto.PartsIds
                    .Where(pdto => partsIdInDb.Contains(pdto.Id))
                    .Select(p => p.Id)
                    .Distinct()
                    .ToList();

                foreach (var partId in parts)
                {

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
            context.SaveChanges();

            return $"Successfully imported {cars.Count}"; 
        }

        //Problem 12:
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<CustomerImportDto>),
                               new XmlRootAttribute("Customers"));


            List<CustomerImportDto> customersDtos;

            using (var reader = new StringReader(inputXml))
            {
                customersDtos = (List<CustomerImportDto>)xmlSerializer.Deserialize(reader);
            }

            var customers = Mapper.Map<List<CustomerImportDto>, List<Customer>>(customersDtos);
            

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }
    }
}