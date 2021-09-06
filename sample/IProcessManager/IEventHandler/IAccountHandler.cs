using Saga;
using System.Threading.Tasks;

namespace IProcessManager
{
    public interface IAccountHandler
    {
        [SagaLogicHandler(Topics.AccountHandler.PreDeductBalance, HandleType.Handle)]
        Task<WordsDto> PreDeductBalance(WordsDto dto);
        [SagaLogicHandler(Topics.AccountHandler.BalanceRollback, HandleType.Rollback)]
        Task<WordsDto> BalanceRollback(WordsDto dto);
    }
}
