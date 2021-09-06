using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.HandleBuilder
{
    public abstract class BaseRequestDelegate
    {
        public string Topic { get; set; }
        public HandleType HandleType { get; set; }
        public abstract Task Excute(SagaData jsonData, IServiceProvider lifetimeScope);
    }
}
