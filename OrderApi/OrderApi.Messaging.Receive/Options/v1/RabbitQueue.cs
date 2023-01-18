namespace OrderApi.Messaging.Receive.Options.v1
{
    public class RabbitQueue
    {
        public string Name { get; set; }
        public IEnumerable<RabbitBinding> Bindings { get; set; }
    }
}
