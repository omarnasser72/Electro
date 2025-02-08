using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.Profiles
{
    public class OrderProfile : BaseProfile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
        }
    }
}
