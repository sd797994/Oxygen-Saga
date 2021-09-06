using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga;
using Saga.HandleBuilder;

namespace Saga.Store.Redis
{
    public static class SagaContainerBuilder
    {
        public static void AddSagaStore(this IServiceCollection services)
        {
            services.AddSingleton<IStoreProvider, StoreProviderRedisImpl>();
        }
    }
}