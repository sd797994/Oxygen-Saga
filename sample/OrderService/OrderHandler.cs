using IProcessManager;
using Microsoft.Extensions.Logging;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService
{
    public class OrderHandler : IOrderHandler
    {
        public async Task<WordsDto> OrderCreate(WordsDto dto)
        {
            if (new Random(Guid.NewGuid().GetHashCode()).Next(0, 2) == 1)
            {
                Console.WriteLine($"{dto.Words}订单创建成功!");
                return await Task.FromResult(new WordsDto() { Words = $"{dto.Words}订单创建成功!" });
            }
            else
                throw new SagaException<WordsDto>(new WordsDto() { Words = $"{dto.Words}订单创建失败!" });
        }
    }
}
