using BeetleX.EventArgs;
using BeetleX.XRPC;
using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using Microsoft.Extensions.Hosting;
using Northwind.Data;
using System;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.UseXRPC(s =>
                    {
                        s.ServerOptions.LogLevel = LogType.Warring;
                        s.RPCOptions.ParameterFormater = new JsonPacket();
                    },
                    rpc =>
                    {
                        Task.Run(() => OnPublishDateTime(rpc));
                        Task.Run(() => OnPublishOrder(rpc));

                    },
                    typeof(Program).Assembly);
                });

            builder.Build().Run();
        }

        static void OnPublishOrder(XRPCServer server)
        {
            var ran = new Random();
            var orders = Northwind.Data.DataHelper.Defalut.Orders;
            while (true)
            {
                var index = ran.Next();

                var order = orders[index % orders.Count];
                server.Publish<Action<Order>>()(order);
                System.Threading.Thread.Sleep(1000);
            }
        }

        static void OnPublishDateTime(XRPCServer server)
        {
            while (true)
            {
                server.Publish<Action<DateTime>>()(DateTime.Now);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
