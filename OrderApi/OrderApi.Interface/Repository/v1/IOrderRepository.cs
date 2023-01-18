using OrderApi.Domain.Entities;

namespace OrderApi.Interface.Repository.v1
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetPaidOrdersAsync(CancellationToken cancellationToken);

        Task<Order> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken);

        Task<List<Order>> GetOrderByCustomerGuidAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
