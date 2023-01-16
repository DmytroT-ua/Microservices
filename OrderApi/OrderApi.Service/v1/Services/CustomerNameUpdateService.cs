using MediatR;
using OrderApi.Domain.Dto;
using OrderApi.Interface.Service.v1;
using OrderApi.Service.v1.Command;
using OrderApi.Service.v1.Query;
using System.Diagnostics;

namespace OrderApi.Service.v1.Services
{
    public class CustomerNameUpdateService : ICustomerNameUpdateService
    {
        private readonly IMediator _mediator;

        public CustomerNameUpdateService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async void UpdateCustomerNameInOrders(UpdateCustomerFullNameDto dto)
        {
            try
            {
                var ordersOfCustomer = await _mediator.Send(new GetOrderByCustomerGuidQuery
                {
                    CustomerId = dto.Id
                });

                if (ordersOfCustomer.Count != 0)
                {
                    ordersOfCustomer.ForEach(x => x.CustomerFullName = $"{dto.FirstName} {dto.LastName}");
                }

                await _mediator.Send(new UpdateOrderCommand
                {
                    Orders = ordersOfCustomer
                });
            }
            catch (Exception ex)
            {
                // log an error message here

                Debug.WriteLine(ex.Message);
            }
        }
    }
}
