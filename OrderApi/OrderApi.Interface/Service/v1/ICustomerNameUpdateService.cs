using OrderApi.Domain.Dto;

namespace OrderApi.Interface.Service.v1
{
    public interface ICustomerNameUpdateService
    {
        void UpdateCustomerNameInOrders(UpdateCustomerFullNameDto dto);
    }
}
