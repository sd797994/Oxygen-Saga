using IProcessManager;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService
{
    public class AccountHandler : IAccountHandler
    {

        public async Task<WordsDto> PreDeductBalance(WordsDto dto)
        {
            if (new Random(Guid.NewGuid().GetHashCode()).Next(0, 2) == 1)
            {
                return await Task.FromResult(new WordsDto() { Words = $"{dto.Words}预扣余额成功!" });
            }
            else
                throw new SagaException<WordsDto>(new WordsDto() { Words = $"{dto.Words}预扣余额失败!" });
        }
        public async Task<WordsDto> BalanceRollback(WordsDto dto)
        {
            if (new Random(Guid.NewGuid().GetHashCode()).Next(0, 2) == 1)
            {
                return await Task.FromResult(new WordsDto() { Words = $"{dto.Words},补偿余额成功!" });
            }
            else
                throw new SagaException<WordsDto>(dto, "补偿余额异常");
        }
    }
}
