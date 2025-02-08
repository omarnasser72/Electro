using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.Profiles
{
    public class CartProductProfile : BaseProfile
    {
        public CartProductProfile()
        {
            CreateMap<CartProduct, CartProductDTO>().ReverseMap();
            CreateMap<CartProduct, CartProductModel>().ReverseMap();
        }
    }
}
