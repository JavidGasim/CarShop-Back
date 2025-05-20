using AutoMapper;
using CarShop.Entities.Entites;
using CarShop.WepApi.DTOS;

namespace CarShop.WepApi.AutoMapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<CustomIdentityUser, UserDto>().ReverseMap();
        }
    }
}
