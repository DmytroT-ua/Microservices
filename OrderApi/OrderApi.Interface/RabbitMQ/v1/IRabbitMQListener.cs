using System.Threading.Tasks;

namespace OrderApi.Interface.RabbitMQ.v1
{
    public interface IRabbitMQListener
    {
        Task<bool> Subscribe(string routingKey, string message);
    }
}
