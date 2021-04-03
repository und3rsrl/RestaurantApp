using AutoMapper;
using RestaurantApp.BusinessEntities.DTOs.Food;
using RestaurantApp.DataModel.Models;

namespace RestaurantApp.BusinessEntities.Profiles
{
    public class FoodProfile : Profile
    {
        public FoodProfile()
        {
            CreateMap<Food, FoodDetails>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
