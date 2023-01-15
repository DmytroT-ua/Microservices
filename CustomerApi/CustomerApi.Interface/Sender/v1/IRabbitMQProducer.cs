namespace CustomerApi.Interface.Sender.v1
{
    public interface IRabbitMQProducer
    {
        void SendMessage<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class;
    }
}
