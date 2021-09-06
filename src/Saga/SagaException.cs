using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class BaseSagaException : Exception
    {
        public object RollbackModel { get; set; }
        public BaseSagaException()
        {

        }
        public BaseSagaException(string message) : base(message)
        {

        }
    }
    public class SagaException<T> : BaseSagaException
    {
        public SagaException(T rollbackModel)
        {
            RollbackModel = rollbackModel;
        }
        public SagaException(T rollbackModel, string message) : base(message)
        {
            RollbackModel = rollbackModel;
        }
    }
}
