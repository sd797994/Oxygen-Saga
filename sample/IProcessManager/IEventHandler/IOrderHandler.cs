using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProcessManager
{
    public interface IOrderHandler
    {
        [SagaLogicHandler(Topics.OrderHandler.OrderCreate, HandleType.Handle)]
        Task<WordsDto> OrderCreate(WordsDto dto);
    }
}
