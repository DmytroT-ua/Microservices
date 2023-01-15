using CustomerApi.Interface.Sender.v1;
using CustomerApi.Messaging.Send.Options.v1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CustomerApi.Messaging.Send.Sender.v1
{
    public class RabbitMQProducer : IRabbitMQProducer, IDisposable
    {
        private readonly IServiceScope _loggerScope;
        private readonly RabbitMqConfiguration _settings;

        private IConnection _connection;
        private IModel _channel;

        public RabbitMQProducer(
            IServiceProvider serviceProvider,
            IOptions<RabbitMqConfiguration> settings)
        {
            //Singleton service, need a scope to use another services
            //_loggerScope = serviceProvider.CreateScope();
            //_log = _loggerScope.ServiceProvider.GetService<ILog>();
            _settings = settings.Value;
        }

        public void SendMessage<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class
        {
            if (message == null)
                return;

            try
            {
                var channel = GetChannel();

                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception ex)
            {
                //_log.LogError("RabbitMQProducer.SendMessageError", ex);
                throw;
            }
        }

        private IModel GetChannel()
        {
            if (_channel == null || _channel.IsClosed)
            {
                _channel = GetConnection().CreateModel();
            }
            return _channel;
        }

        private IConnection GetConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.Hostname,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    Port = _settings.Port,
                    DispatchConsumersAsync = true,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(_settings.NetworkRecoveryIntervalSecond)
                };

                _connection = factory.CreateConnection();
            }

            return _connection;
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
            _loggerScope.Dispose();
        }
    }
}
