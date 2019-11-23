using AutoMapper;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierImportDto, Supplier>();

            CreateMap<PartImportDto, Part>();

            CreateMap<CarImportDto, Car>();
        }
    }
}
