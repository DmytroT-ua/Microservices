using CustomerApi.Domain.Entities;

namespace CustomerApi.Interface.Sender.v1
{
    public interface ICustomerUpdateSender
    {
        void SendCustomer(Customer customer);
    }
}
