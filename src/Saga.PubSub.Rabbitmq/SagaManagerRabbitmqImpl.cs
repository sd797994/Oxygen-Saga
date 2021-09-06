using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Saga.PubSub.Rabbitmq
{
    public class SagaManagerRabbitmqImpl : ISagaManager
    {
        IModel RabbitClient;
        private readonly SagaConfiguration sagaConfiguration;
        public SagaManagerRabbitmqImpl()
        {
            sagaConfiguration = ConfigurationManager.GetConfig();
            RabbitClient = RabbitmqClientFactory.GetClient();
        }
        public async Task StartOrNext<T>(string topic, T data)
        {
            RabbitClient.ExchangeDeclare(sagaConfiguration.ClusterName, "topic");
            RabbitClient.BasicPublish(sagaConfiguration.ClusterName, topic, null, JsonSerializer.SerializeToUtf8Bytes(new SagaData(sagaConfiguration.GetflowNameByTopic(topic), topic, data)));
            await Task.CompletedTask;
        }
    }
}
