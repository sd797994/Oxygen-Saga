using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace Saga
{
    public class SagaData
    {
        public SagaData() { }
        public SagaData(string flowName, string topic, object data)
        {
            FlowName = flowName;
            Topic = topic;
            Data = JsonSerializer.SerializeToUtf8Bytes(data);
            StoreKey = Guid.NewGuid().ToString();
            StoreState = SagaDataState.Processing;
        }
        public string StoreKey { get; set; }
        public SagaDataState StoreState { get; set; }
        /// <summary>
        /// 流程名
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 当前主题
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 事件主体
        /// </summary>
        public byte[] Data { get; set; }

        public void SetState(SagaDataState storeState) => StoreState = storeState;
    }
    public enum SagaDataState
    {
        Processing = 0,
        Done = 1,
        Error = 2
    }
}
