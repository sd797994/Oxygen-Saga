using Saga;
using Saga.HandleBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProcessManager
{
    public struct Topics
    {
        public struct GoodsHandler
        {
            /// <summary>
            /// 预扣库存
            /// </summary>
            public const string PreDeductInventory = "GoodsService.PreDeductInventory";
            /// <summary>
            /// 库存回滚
            /// </summary>
            public const string InventoryRollback = "GoodsService.InventoryRollback";

        }
        public struct AccountHandler 
        {
            /// <summary>
            /// 预扣余额
            /// </summary>
            public const string PreDeductBalance = "AccountService.PreDeductBalance";
            /// <summary>
            /// 余额回滚
            /// </summary>
            public const string BalanceRollback = "AccountService.BalanceRollback";
        }

        public struct OrderHandler 
        {
            /// <summary>
            /// 订单生成
            /// </summary>
            public const string OrderCreate = "OrderService.OrderCreate";
        }
    }
}
