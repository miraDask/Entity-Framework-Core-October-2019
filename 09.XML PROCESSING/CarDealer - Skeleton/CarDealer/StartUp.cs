using System;
using System.Collections.Generic;
using System.IO;
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

                var inputXml = File.ReadAllText("./../../../Datasets/suppliers.xml");
                Console.WriteLine(ImportSuppliers(context, inputXml));

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
    }
}