using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.Profiles
{
    public class CategoryProfile : BaseProfile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
