using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Common.Implements;
using Oxygen.Common.Interface;
using Saga.HandleBuilder;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Saga.PubSub.Dapr
{
    public static class SagaContainerBuilder
    {
        public static void AddSaga(this IServiceCollection services, SagaConfiguration sagaConfiguration)
        {
            services.AddSingleton<ISagaManager, SagaManagerDaprImpl>();
            services.AddSingleton<ISagaEventHandler, SagaEventHandlerDaprImpl>();
            ConfigurationManager.SetConfig(sagaConfiguration);
        }
        public static void RegisterSagaHandler(this IApplicationBuilder applicationbuilder, Func<ErrorModel, Task> errorHandle)
        {
            //注册中间件用于写入saga特定的eventhandle到dapr的/dapr/subscribe路由终结点用于dapr注册订阅器
            applicationbuilder.UseMiddleware<DparSubscribeMiddleware>();
            HandleProxyFactory.RegiterAllHandle(errorHandle);
            applicationbuilder.ApplicationServices.GetService<ISagaEventHandler>().ConsumerReceivedHandle(applicationbuilder);
        }
    }
}
