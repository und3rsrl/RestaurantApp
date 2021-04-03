using AutoMapper;
using RestaurantApp.BusinessEntities.DTOs.Category;
using RestaurantApp.DataModel.Models;

namespace RestaurantApp.BusinessEntities.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDetails>().ReverseMap();
        }
    }
}
