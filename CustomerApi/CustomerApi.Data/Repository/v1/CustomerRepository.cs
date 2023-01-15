using CustomerApi.Data.Database;
using CustomerApi.Domain.Entities;
using CustomerApi.Interface.Repository.v1;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Data.Repository.v1
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await DatabaseContext.Customer.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
