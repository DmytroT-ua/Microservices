using MediatR;
using OrderApi.Domain.Entities;
using OrderApi.Interface.Repository.v1;

namespace OrderApi.Service.v1.Command
{
    public class PayOrderCommandHandler : IRequestHandler<PayOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public PayOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Handle(PayOrderCommand request, CancellationToken cancellationToken)
        {
            return await _orderRepository.UpdateAsync(request.Order);
        }
    }
}