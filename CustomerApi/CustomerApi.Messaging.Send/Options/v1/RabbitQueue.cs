namespace CustomerApi.Messaging.Send.Options.v1
{
    public class RabbitQueue
    {
        public string Name { get; set; }
        public IEnumerable<RabbitBinding> Bindings { get; set; }
    }
}
