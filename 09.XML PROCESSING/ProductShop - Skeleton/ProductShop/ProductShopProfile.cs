using AutoMapper;
using ProductShop.Dtos.Export;
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

            CreateMap<Product, ProductInRangeDto>()
                .ForMember(x => x.Buyer,
                           y => y.MapFrom(p => $"{p.Buyer.FirstName} {p.Buyer.LastName}"));
        }
    }
}
