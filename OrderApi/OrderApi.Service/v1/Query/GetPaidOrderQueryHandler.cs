using MediatR;
using OrderApi.Domain.Entities;
using OrderApi.Interface.Repository.v1;

namespace OrderApi.Service.v1.Query
{
    public class GetPaidOrderQueryHandler : IRequestHandler<GetPaidOrderQuery, List<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetPaidOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Order>> Handle(GetPaidOrderQuery request, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetPaidOrdersAsync(cancellationToken);
        }
    }
}