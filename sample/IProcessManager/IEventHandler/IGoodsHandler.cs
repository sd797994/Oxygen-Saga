using Saga;
using System;
using System.Threading.Tasks;

namespace IProcessManager
{

    public interface IGoodsHandler
    {
        [SagaLogicHandler(Topics.GoodsHandler.PreDeductInventory, HandleType.Handle)]
        Task<WordsDto> DeductInventory(WordsDto dto);
        [SagaLogicHandler(Topics.GoodsHandler.InventoryRollback, HandleType.Rollback)]
        Task InventoryRollback(WordsDto dto);
    }
}
