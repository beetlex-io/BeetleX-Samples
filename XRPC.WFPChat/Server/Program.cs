using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using BeetleX.XRPC;
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
                    s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Debug;
                    s.ServerOptions.DefaultListen.Port = 9090;
                    s.RPCOptions.ParameterFormater = new JsonPacket();//default messagepack
                    s.RPCDisconnect += (o, e) =>
                    {
                        foreach (var session in e.Server.GetOnlines())
                        {
                            if (session != e.Session && !string.IsNullOrEmpty(session.Name))
                            {
                                IUser user = s.GetClient<IUser>(session);
                                user.Exit(e.Session.Name);
                            }
                        }
                    };
                },
                    typeof(Program).Assembly);
            });
            builder.Build().Run();
        }
    }
    public interface IUser
    {
        Task Login(string name);

        Task Talk(string name, string message);

        Task Exit(string name);

    }
    [EventNext.Service(typeof(IUser))]
    public class UserImpl : IUser
    {
        public Task Exit(string name)
        {
            return Task.CompletedTask;
        }

        public Task Login(string name)
        {
            var token = XRPCServer.EventToken;
            token.Session.Name = name;
            foreach (var session in token.Server.Server.GetOnlines())
            {
                if (!string.IsNullOrEmpty(session.Name))
                {
                    IUser user = token.Server.GetClient<IUser>(session);
                    user.Login(name);
                }
            }
            return Task.CompletedTask;
        }

        public Task Talk(string name, string message)
        {
            var token = XRPCServer.EventToken;
            if (string.IsNullOrEmpty(token.Session.Name))
            {
                throw new Exception("登陆无效！");
            }
            foreach (var session in token.Server.Server.GetOnlines())
            {
                if (!string.IsNullOrEmpty(session.Name))
                {
                    IUser user = token.Server.GetClient<IUser>(session);
                    user.Talk(session.Name, message);
                }
            }
            return Task.CompletedTask;
        }
    }

}
