using IProcessManager;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodsService
{
    public class GoodsHandler : IGoodsHandler
    {
        public async Task<WordsDto> DeductInventory(WordsDto dto)
        {
            if (new Random(Guid.NewGuid().GetHashCode()).Next(0, 2) == 1)
            {
                return await Task.FromResult(new WordsDto() { Words = $"{dto.Words}预扣库存成功!" });
            }
            else
            {
                Console.WriteLine("预扣库存失败");
                throw new SagaException<WordsDto>(dto);
            }
        }

        public async Task InventoryRollback(WordsDto dto)
        {
            if (new Random(Guid.NewGuid().GetHashCode()).Next(0, 2) == 1)
            {
                Console.WriteLine($"{dto.Words},补偿库存成功!");
                await Task.CompletedTask;
            }
            else
                throw new SagaException<WordsDto>(dto, "补偿库存异常");
        }
    }
}
