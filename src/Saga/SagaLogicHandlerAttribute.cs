using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SagaLogicHandlerAttribute : Attribute
    {
        public string Topic { get; set; }
        public HandleType HandleType { get; set; }
        public SagaLogicHandlerAttribute(string topic, HandleType handleType)
        {
            Topic = topic;
            HandleType = handleType;
        }
    }
    public enum HandleType
    {
        Handle = 0,
        Rollback = 1
    }
}
