using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public interface ISagaManager
    {
        Task StartOrNext<T>(string topic, T data);
    }
}
