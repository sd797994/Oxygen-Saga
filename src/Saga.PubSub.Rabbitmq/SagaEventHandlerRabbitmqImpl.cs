using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Saga.HandleBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Saga.PubSub.Rabbitmq
{
    public class SagaEventHandlerRabbitmqImpl : ISagaEventHandler
    {
        IModel RabbitClient;
        EventingBasicConsumer eventingBasicConsumer;
        SagaConfiguration sagaConfiguration;
        public SagaEventHandlerRabbitmqImpl()
        {
            sagaConfiguration = ConfigurationManager.GetConfig();
            RabbitClient = RabbitmqClientFactory.GetClient();
            eventingBasicConsumer = new EventingBasicConsumer(RabbitClient);
        }
        public void ConsumerReceivedHandle(IApplicationBuilder applicationBuilder)
        {
            eventingBasicConsumer.Received += async (ch, args) => await HandAsync(args, applicationBuilder.ApplicationServices);
            RabbitClient.ExchangeDeclare(sagaConfiguration.ClusterName, "topic");
            RabbitClient.QueueDeclare(sagaConfiguration.ServiceName, false, false, false, null);
            RabbitClient.QueueBind(sagaConfiguration.ServiceName, sagaConfiguration.ClusterName, $"{sagaConfiguration.ServiceName}.#");
            RabbitClient.BasicConsume(sagaConfiguration.ServiceName, false, eventingBasicConsumer);
        }
        async Task HandAsync(BasicDeliverEventArgs args, IServiceProvider rootProvider)
        {
            //为每次订阅处理器构建独立的生命周期
            using (var scope = rootProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                SagaData data = default;
                var storeProvider = serviceProvider.GetService<IStoreProvider>();
                try
                {
                    data = JsonSerializer.Deserialize<SagaData>(args.Body.Span);
                    var oldData = await storeProvider.GetKey(data.StoreKey);
                    if (oldData == null || oldData.StoreState == SagaDataState.Error)
                    {
                        data.SetState(SagaDataState.Processing);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                        await HandleProxyFactory.GetDelegate().FirstOrDefault(x => x.Topic == args.RoutingKey).Excute(data, serviceProvider);
                        data.SetState(SagaDataState.Done);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                    }
                }
                catch (Exception e)
                {
                    if (data != default)
                    {
                        data.SetState(SagaDataState.Error);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                    }
                }
                finally
                {
                    RabbitClient.BasicAck(args.DeliveryTag, false);
                }
            }
        }
    }
}
