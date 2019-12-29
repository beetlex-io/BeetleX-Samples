using BeetleX.EventArgs;
using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using EventNext;
using Microsoft.Extensions.Hosting;
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
                    s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Trace;
                    s.ServerOptions.DefaultListen.Port = 9090;
                    s.RPCOptions.ParameterFormater = new JsonPacket();
                },
                    typeof(Program).Assembly);
            });
            builder.Build().Run();
        }
    }

    public interface IHello
    {
        Task<string> Hello(string name);
    }

    [Service(typeof(IHello))]
    public class HelloImpl : IHello
    {
        public Task<string> Hello(string name)
        {
            return $"hello {name} {DateTime.Now}".ToTask();
        }
    }

}
