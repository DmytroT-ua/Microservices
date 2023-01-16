using AutoMapper;
using OrderApi.Domain.Entities;
using OrderApi.Model.v1;

namespace OrderApi.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderModel, Order>()
                .ForMember(x => x.OrderState, opt => opt.MapFrom(src => 1));
        }
    }
}