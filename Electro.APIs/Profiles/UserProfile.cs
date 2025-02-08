using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;

namespace Electro.APIs.Profiles
{
    public class UserProfile : BaseProfile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, Customer>().ReverseMap();
            CreateMap<Customer, UserDTO>().ReverseMap();

            CreateMap<RegisterModel, Admin>().ReverseMap();
            CreateMap<Admin, UserDTO>().ReverseMap();

            CreateMap<LoginModel, Customer>().ReverseMap();
            CreateMap<LoginModel, Admin>().ReverseMap();
        }
    }
}
