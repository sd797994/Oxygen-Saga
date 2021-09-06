using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProcessManager
{
    public class CreateOrderTopicConfiguration : TopicConfiguration
    {
        public override string FlowName { get; set; } = "OrderCreate";
        public CreateOrderTopicConfiguration()
        {
            AddNext(Topics.GoodsHandler.PreDeductInventory, Topics.GoodsHandler.InventoryRollback)
                .AddNext(Topics.AccountHandler.PreDeductBalance, Topics.AccountHandler.BalanceRollback)
                .AddNext(Topics.OrderHandler.OrderCreate);
        }
    }
}
