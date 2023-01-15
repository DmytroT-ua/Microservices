using AutoMapper;
using CustomerApi.Domain.Entities;
using CustomerApi.Models.v1;

namespace CustomerApi.AutoMapper.v1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerModel, Customer>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<UpdateCustomerModel, Customer>();
        }
    }
}
