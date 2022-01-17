using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Store.Dapr
{
    public class DaprStateStoreSagaModel<T> : StateStore
    {
        public DaprStateStoreSagaModel(string key, T data, int expireTimeSecond)
        {
            Key = $"DaprStateStoreSagaModel_{key}";
            this.Data = data;
            TtlInSeconds = expireTimeSecond;
        }
        public DaprStateStoreSagaModel(string key)
        {
            Key = $"DaprStateStoreSagaModel_{key}";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
}
