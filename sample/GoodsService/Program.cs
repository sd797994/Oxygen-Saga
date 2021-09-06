using Autofac;
using Autofac.Extensions.DependencyInjection;
using IProcessManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Oxygen.IocModule;
using Oxygen.Server.Kestrel.Implements;
using Saga;
using Saga.PubSub.Dapr;
using Saga.Store.Dapr;
using System;
using System.Text;

namespace GoodsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateDefaultHost(args).Build().Run();
        }

        static IHostBuilder CreateDefaultHost(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webhostbuilder =>
            {
                //注册成为oxygen服务节点
                webhostbuilder.StartOxygenServer<OxygenStartup>((config) =>
                {
                    config.Port = 80;
                    config.PubSubCompentName = "pubsub";
                    config.StateStoreCompentName = "statestore";
                    config.TracingHeaders = "Authentication";
                    config.UseCors = true;
                });
                webhostbuilder.ConfigureServices(services =>
                {
                    services.AddScoped<IGoodsHandler, GoodsHandler>();
                    services.AddSaga(new SagaConfiguration("EshopSample", "GoodsService", "amqp://guest:123456@192.168.1.253:5672", "127.0.0.1:6379,prefix=test_", new CreateOrderTopicConfiguration()));
                    services.AddSagaStore();
                    services.AddControllers();
                }).
                Configure((ctx, app) =>
                {
                    if (ctx.HostingEnvironment.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                    }
                    app.Map("/start", builder => builder.Run(async ctx =>
                    {
                        await ctx.RequestServices.GetService<ISagaManager>().StartOrNext(Topics.GoodsHandler.PreDeductInventory, new WordsDto() { Words = "启动订单创建;" });
                        await ctx.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("ok"));
                    }));
                    app.UseRouting();
                    app.UseAuthorization();
                    app.RegisterSagaHandler((x) => { Console.WriteLine($"主题{x.SourceTopic}消费异常,原始数据json:{x.SourceDataJson},{(x.ErrorDataJson == null ? "" : $"异常回调json:{x.ErrorDataJson}，")}需要人工处理"); });
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            })
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                //注入oxygen依赖
                builder.RegisterOxygenModule();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddAutofac();
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
