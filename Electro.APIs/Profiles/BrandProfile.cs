using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.Profiles
{
    public class BrandProfile : BaseProfile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandModel>().ReverseMap();
            CreateMap<Brand, BrandDTO>();
        }
    }
}
