using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.Profiles
{
    public class OrderProductProfile : BaseProfile
    {
        public OrderProductProfile()
        {
            CreateMap<OrderProduct, OrderProductModel>().ReverseMap();
            CreateMap<OrderProduct, OrderProductDTO>().ReverseMap();
        }
    }
}
