using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.Profiles
{
    public class ProductProfile : BaseProfile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(PD => PD.BrandName, MemberOptions => MemberOptions.MapFrom(P => P.Brand.Name))
                .ForMember(PD => PD.CategoryName, MemberOptions => MemberOptions.MapFrom(P => P.Category.Name))
                .ReverseMap();

            CreateMap<Product, ProductModel>().ReverseMap();
        }
    }
}
