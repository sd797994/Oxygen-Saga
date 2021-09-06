using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Store.Dapr
{
    public class StoreProviderDaprImpl : IStoreProvider
    {
        private readonly IStateManager stateManager;
        public StoreProviderDaprImpl(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
        public async Task<SagaData> GetKey(string key)
        {
            return await stateManager.GetState<SagaData>(new DaprStateStoreSagaModel<SagaData>(key));
        }

        public async Task<bool> RemoveKey(string key)
        {
            await stateManager.DelState(new DaprStateStoreSagaModel<SagaData>(key));
            return true;
        }

        public async Task<bool> SetDataByKey(string key, SagaData data, DateTime expireTime)
        {
            await stateManager.SetState(new DaprStateStoreSagaModel<SagaData>(key, data, (expireTime - DateTime.Now).TotalSeconds> int.MaxValue ? -1: (int)((expireTime - DateTime.Now).TotalSeconds)));
            return true;
        }
    }
}
