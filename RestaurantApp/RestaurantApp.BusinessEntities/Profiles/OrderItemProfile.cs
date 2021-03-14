using AutoMapper;
using RestaurantApp.BusinessEntities.DTOs.Order;
using RestaurantApp.DataModel.Models;

namespace RestaurantApp.BusinessEntities.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        }
    }
}
