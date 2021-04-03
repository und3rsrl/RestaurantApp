using AutoMapper;
using RestaurantApp.BusinessEntities.DTOs.Order;
using RestaurantApp.BusinessEntities.DTOs.Waiter;
using RestaurantApp.DataModel.Models;

namespace RestaurantApp.BusinessEntities.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderItem, OrderItemDetails>()
                .ReverseMap();

            CreateMap<Order, PreviousOrderDetails>()
                .ForMember(dest => dest.SubmitDate, ops => ops.MapFrom(src => src.SubmitDateTime))
                .ReverseMap();

            CreateMap<Order, WaiterOrderInfoDetails>()
                .ReverseMap();
        }
    }
}
