using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using EventNext;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using BeetleX.XRPC;
namespace Server
{
    [Service(typeof(IUser))]
    public class Program : IUser
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.UseXRPC(s =>
                {
                    s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Warring;
                    s.ServerOptions.DefaultListen.Port = 9090;
                    s.RPCOptions.ParameterFormater = new JsonPacket();//default messagepack
                },
                    typeof(Program).Assembly);
            });
            builder.Build().Run();
        }

        public Task<DateTime> GetTime()
        {
            return DateTime.Now.ToTask();
        }

        public Task Login(string name)
        {
            Console.WriteLine($"{name} login");
            var token = XRPCServer.EventToken;
            Task.Run(async () =>
            {
                IUser user = token.Server.GetClient<IUser>(token.Session);
                while (true)
                {
                    var time = await user.GetTime();
                    Console.WriteLine($"{name}[{token.Session.RemoteEndPoint}] time is:{time}");
                    //await Task.Delay(1000);
                }
            });
            return Task.CompletedTask;
        }
    }

    public interface IUser
    {
        Task Login(string name);

        Task<DateTime> GetTime();
    }


}
