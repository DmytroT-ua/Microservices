namespace CustomerApi.Messaging.Send.Options.v1
{
    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int NetworkRecoveryIntervalSecond { get; set; }
        public IEnumerable<RabbitQueue> Queues { get; set; }
    }
}
