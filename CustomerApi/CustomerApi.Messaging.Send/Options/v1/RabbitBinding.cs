namespace CustomerApi.Messaging.Send.Options.v1
{
    public class RabbitBinding
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public IEnumerable<string> RoutingKeys { get; set; }
    }
}
