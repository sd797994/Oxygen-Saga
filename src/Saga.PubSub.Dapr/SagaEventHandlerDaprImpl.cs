using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Oxygen.Client.ServerSymbol.Events;
using Oxygen.Common.Implements;
using Oxygen.Common.Interface;
using Saga.HandleBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Saga.PubSub.Dapr
{
    public class SagaEventHandlerDaprImpl : ISagaEventHandler
    {
        public void ConsumerReceivedHandle(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Map($"/SagaSubscribe/{ConfigurationManager.GetConfig().ServiceName}", builder => builder.Run(async ctx => await SagaSubscribeHandle(ctx)));
        }

        async Task SagaSubscribeHandle(HttpContext context)
        {
            HttpContextExtension.ContextWapper.Value = new OxygenHttpContextWapper($"/SagaSubscribe/{ConfigurationManager.GetConfig().ServiceName}", context.RequestServices.GetService<ILifetimeScope>(), context);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 200;
            var storeProvider = context.RequestServices.GetService<IStoreProvider>();
            var serialize = context.RequestServices.GetService<ISerialize>();
            OxygenIocContainer.BuilderIocContainer(context.RequestServices.GetService<ILifetimeScope>());
            using (var stream = new MemoryStream())
            {
                await context.Request.Body.CopyToAsync(stream);
                SagaData data = serialize.DeserializesJson<TempDataByEventHandleInput<SagaData>>(Encoding.UTF8.GetString(stream.ToArray())).GetData();
                try
                {
                    var oldData = await storeProvider.GetKey(data.StoreKey);
                    if (oldData == null || oldData.StoreState == SagaDataState.Error)
                    {
                        data.SetState(SagaDataState.Processing);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                        await HandleProxyFactory.GetDelegate().FirstOrDefault(x => x.Topic == data.Topic).Excute(data, context.RequestServices);
                        data.SetState(SagaDataState.Done);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                    }
                }
                catch (Exception e)
                {
                    if (data != default)
                    {
                        data.SetState(SagaDataState.Error);
                        await storeProvider.SetDataByKey(data.StoreKey, data, DateTime.Now.AddDays(1));
                    }
                }
                finally
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(DefaultEventHandlerResponse.Default()));
                }
            }

        }
    }
}