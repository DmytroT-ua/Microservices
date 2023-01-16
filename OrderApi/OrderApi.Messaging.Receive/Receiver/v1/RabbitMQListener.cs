using Newtonsoft.Json;
using OrderApi.Domain.Const;
using OrderApi.Domain.Dto;
using OrderApi.Interface.RabbitMQ.v1;
using OrderApi.Interface.Service.v1;

namespace OrderApi.Messaging.Receive.Receiver.v1
{
    public class RabbitMQListener : IRabbitMQListener
    {
        private readonly ICustomerNameUpdateService _nameUpdateService;

        public RabbitMQListener(
            ICustomerNameUpdateService nameUpdateService)
        {
            _nameUpdateService = nameUpdateService;
        }

        public async Task<bool> Subscribe(string routingKey, string message)
        {
            bool success = false;
            if (routingKey.StartsWith(RabbitConst.OrderApiQueueName))
            {
                //for full flow here should be messages validator that returns orders and isValid

                var updateCustomerDto = JsonConvert.DeserializeObject<UpdateCustomerFullNameDto>(message);
                _nameUpdateService.UpdateCustomerNameInOrders(updateCustomerDto);
                success = true;
            }

            return success;
        }
    }
}
