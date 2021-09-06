using Oxygen.Client.ServerProxyFactory.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.PubSub.Dapr
{
    public class SagaManagerDaprImpl : ISagaManager
    {
        private readonly IEventBus eventBus;
        private readonly SagaConfiguration sagaConfiguration;
        public SagaManagerDaprImpl(IEventBus eventBus)
        {
            sagaConfiguration = ConfigurationManager.GetConfig();
            this.eventBus = eventBus;
        }
        public async Task StartOrNext<T>(string topic, T data)
        {
            await eventBus.SendEvent($"{topic.Split(".")[0]}", new SagaData(sagaConfiguration.GetflowNameByTopic(topic), topic, data));
        }
    }
}
