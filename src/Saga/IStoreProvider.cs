using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public interface IStoreProvider
    {
        Task<bool> SetDataByKey(string key, SagaData data, DateTime expireTime);
        Task<bool> RemoveKey(string key);
        Task<SagaData> GetKey(string key);
    }
}
