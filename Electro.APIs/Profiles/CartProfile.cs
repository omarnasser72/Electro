using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;
using System.Reflection;

namespace Electro.APIs.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartModel>().ReverseMap();
            CreateMap<Cart, CartDTO>()
                .ForMember(CartDTO => CartDTO.CustomerName, memberOptions => memberOptions.MapFrom(Cart => Cart.Customer.Name));

        }
    }
}
