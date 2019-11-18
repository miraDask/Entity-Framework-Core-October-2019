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
            CreateMap<Customer, customerDto>()
                .ForMember(x => x.BirthDate, y => y.MapFrom(c => c.BirthDate.ToString("dd/MM/yyyy")));
        }
    }
}
