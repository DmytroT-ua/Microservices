using Microsoft.EntityFrameworkCore;
using OrderApi.Data.Database;
using OrderApi.Domain.Entities;
using OrderApi.Interface.Repository.v1;

namespace OrderApi.Data.Repository.v1
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<List<Order>> GetPaidOrdersAsync(CancellationToken cancellationToken)
        {
            return await DatabaseContext.Order.Where(x => x.OrderState == 2).ToListAsync(cancellationToken);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await DatabaseContext.Order.FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
        }

        public async Task<List<Order>> GetOrderByCustomerGuidAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await DatabaseContext.Order.Where(x => x.CustomerGuid == customerId).ToListAsync(cancellationToken);
        }
    }
}
