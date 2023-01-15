using CustomerApi.Domain.Const;
using CustomerApi.Domain.Entities;
using CustomerApi.Interface.Sender.v1;
using CustomerApi.Messaging.Send.Options.v1;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace CustomerApi.Messaging.Send.Sender.v1
{
    public class CustomerUpdateSender : ICustomerUpdateSender
    {
        private readonly RabbitBinding _queueSettings;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public CustomerUpdateSender(
            IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IRabbitMQProducer rabbitMQProducer)
        {
            _queueSettings = rabbitMqOptions.Value.Queues
                .FirstOrDefault(x => x.Name == RabbitConst.OrderApiQueueName)
                ?.Bindings?.First();
            _rabbitMQProducer = rabbitMQProducer;
        }

        public void SendCustomer(Customer customer)
        {
            var json = JsonConvert.SerializeObject(customer);
            var body = Encoding.UTF8.GetBytes(json);

            _rabbitMQProducer.SendMessage(
                customer, 
                _queueSettings.ExchangeName, 
                _queueSettings.ExchangeType,
                _queueSettings.RoutingKeys.First());
        }
    }
}
