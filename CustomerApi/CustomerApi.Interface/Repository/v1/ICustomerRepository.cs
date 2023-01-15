using CustomerApi.Domain.Entities;

namespace CustomerApi.Interface.Repository.v1
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
