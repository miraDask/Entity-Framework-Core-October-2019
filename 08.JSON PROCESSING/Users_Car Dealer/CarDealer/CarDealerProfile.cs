using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(x => x.BirthDate, y => y.MapFrom(c => c.BirthDate.ToString("dd/MM/yyyy")));

            CreateMap<Car, CarToJsonDto>();

            CreateMap<Supplier, LocalSupplierDto>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(s => s.Parts.Count));
        }
    }
}
