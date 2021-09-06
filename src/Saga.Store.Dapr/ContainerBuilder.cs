using Microsoft.Extensions.DependencyInjection;
using System;

namespace Saga.Store.Dapr
{
    public static class SagaContainerBuilder
    {
        public static void AddSagaStore(this IServiceCollection services)
        {
            services.AddSingleton<IStoreProvider, StoreProviderDaprImpl>();
        }
    }
}
