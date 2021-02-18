using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using BeetleX.XRPC;
using EventNext;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ServerApp
{
class Program
{
    static void Main(string[] args)
    {
        var builder = new HostBuilder()
        .ConfigureServices((hostContext, services) =>
        {
            //开
            services.UseXRPC(
                s =>
                {
                    s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Info;
                    s.ServerOptions.DefaultListen.Port = 9090;
                    s.ServerOptions.DefaultListen.SSL = true;//开启SSL
                    s.ServerOptions.DefaultListen.CertificateFile = "test.pfx";
                    s.ServerOptions.DefaultListen.CertificatePassword = "123456";
                    s.RPCOptions.ParameterFormater = new JsonPacket();//default messagepack
                },
                s =>
                {
                    //绑定一个委托
                    s.AddDelegate<Func<Task<string>>>(() =>
                    {
                        return Task.FromResult($"{Environment.OSVersion} {DateTime.Now}");
                    });
                }
                , typeof(Program).Assembly);
        });
        builder.Build().Run();
    }
}
//定义登陆接口
public interface IUser
{
    Task<string> Login(string name, string pwd);
}

[Service(typeof(IUser))]
public class HelloImpl : IUser
{
    //实现登陆方法
    public Task<string> Login(string name, string pwd)
    {
        var context = XRPCServer.EventToken;
        GetClientTime(context.Server, context.Session);
        Console.WriteLine($"{name} login at {context.Session.RemoteEndPoint}");
        return $"{name} login {DateTime.Now}".ToTask();
    }
    //定时从登陆客户端获取系统和时间信息并打印
    private async Task GetClientTime(XRPCServer server, BeetleX.ISession session)
    {
        var action = server.Delegate<Func<Task<string>>>(session);
        while (true)
        {
            var result = await action();
            Console.WriteLine($"{result}@{session.RemoteEndPoint}");
            await Task.Delay(1000);
        }
    }
}

}
