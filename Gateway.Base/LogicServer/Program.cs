using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using System.Linq;
namespace LogicalServer
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HttpServerHosted>();

                });
            builder.Build().Run();
        }
    }

    public class HttpServerHosted : IHostedService
    {
        private HttpApiServer mApiServer;

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            mApiServer = new BeetleX.FastHttpApi.HttpApiServer();

            mApiServer.Options.ServerTag = "Unknow server";
            mApiServer.Options.LogLevel = BeetleX.EventArgs.LogType.Warring;
            mApiServer.Register(typeof(Home).Assembly);
            mApiServer.Open();
            return Task.CompletedTask;
        }
        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            mApiServer.Dispose();
            return Task.CompletedTask;
        }
    }
    [Controller]
    public class Home
    {
        public object Customers(int count, IHttpContext context)
        {
            return Northwind.Data.DataHelper.Defalut.Customers.Take(count);
        }
    }
}
