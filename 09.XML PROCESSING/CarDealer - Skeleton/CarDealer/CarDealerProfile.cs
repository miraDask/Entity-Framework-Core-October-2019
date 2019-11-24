using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierImportDto, Supplier>();

            CreateMap<PartImportDto, Part>();

            CreateMap<CarImportDto, Car>();

            CreateMap<CustomerImportDto, Customer>()
                .ForMember(x => x.BirthDate, y => y.MapFrom(c => DateTime.Parse(c.BirthDate)));

            CreateMap<SaleImportDto, Sale>();

            CreateMap<Car, CarExportDto>();
        }
    }
}
