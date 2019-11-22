﻿using AutoMapper;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserImportDto, User>();

            CreateMap<ProductImportDto, Product>();

            CreateMap<CategoryImportDto, Category>();

            CreateMap<CategoryProductImportDto, CategoryProduct>();
        }
    }
}
