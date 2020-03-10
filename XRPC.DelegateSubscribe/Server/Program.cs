using BeetleX.EventArgs;
using BeetleX.XRPC;
using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static XRPCServer mXRPCServer;

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
                      Task.Run(() => OnPublish(rpc));

                  },
                  typeof(Program).Assembly);
              });

            builder.Build().Run();
        }

        static void OnPublish(XRPCServer server)
        {
            while (true)
            {
                server.Publish<Action<DateTime>>()(DateTime.Now);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
