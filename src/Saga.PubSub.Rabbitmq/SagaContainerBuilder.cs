using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga;
using Saga.HandleBuilder;
using Microsoft.AspNetCore.Builder;

namespace Saga.PubSub.Rabbitmq
{
    public static class SagaContainerBuilder
    {
        public static void AddSaga(this IServiceCollection services, SagaConfiguration sagaConfiguration)
        {   
            services.AddSingleton<ISagaManager, SagaManagerRabbitmqImpl>();
            services.AddSingleton<ISagaEventHandler, SagaEventHandlerRabbitmqImpl>();
            ConfigurationManager.SetConfig(sagaConfiguration);
        }
        public static void RegisterSagaHandler(this IApplicationBuilder applicationbuilder, Func<IServiceProvider, ErrorModel, Task> errorHandle)
        {
            HandleProxyFactory.RegiterAllHandle(errorHandle);
            applicationbuilder.ApplicationServices.GetService<ISagaEventHandler>().ConsumerReceivedHandle(applicationbuilder);
        }
    }
}
