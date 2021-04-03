using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using RestaurantApp.BusinessEntities.DTOs.Waiter;

namespace RestaurantApp.BusinessEntities.Profiles
{
    public class WaiterProfile : Profile
    {
        public WaiterProfile()
        {
            CreateMap<IdentityUser, WaiterDetails>()
                .ForMember(dest => dest.Email, ops => ops.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserId, ops => ops.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
