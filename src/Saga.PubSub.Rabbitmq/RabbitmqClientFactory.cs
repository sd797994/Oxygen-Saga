using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.PubSub.Rabbitmq
{
    public class RabbitmqClientFactory
    {
        static Lazy<IModel> _RabbitClient = new Lazy<IModel>(() => {
            var channel = new ConnectionFactory() { Uri = new Uri(ConfigurationManager.GetConfig().MessageQueueConnectionString) }.CreateConnection().CreateModel();
            return channel;
        });
        public static IModel GetClient()
        {
            return _RabbitClient.Value;
        }
    }
}
