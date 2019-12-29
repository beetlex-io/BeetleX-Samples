using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using Microsoft.Extensions.Hosting;
using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventNext;
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
                    JWTHelper.Init();
                    s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Trace;
                    s.ServerOptions.DefaultListen.Port = 9090;
                    s.RPCOptions.ParameterFormater = new JsonPacket();//default messagepack
                },
                    typeof(Program).Assembly);
            });
            builder.Build().Run();
        }
    }

    public interface IDataService
    {
        Task<string> Login(string name, string pwd);

        Task<List<Employee>> List();
    }
    [Service(typeof(IDataService))]
    [TokenFilter]
    public class DataServiceImpl : IDataService
    {
        public Task<List<Employee>> List()
        {
            return DataHelper.Defalut.Employees.ToTask();
        }

        [SkipActionFilter(typeof(TokenFilter))]
        public Task<string> Login(string name, string pwd)
        {
            string token = null;
            if (name == "admin" && pwd == "123456")
                token = JWTHelper.Default.CreateToken(name, "admin");
            return token.ToTask();

        }
    }

    public class TokenFilter : ActionFilterAttribute
    {
        public override bool Executing(EventCenter center, EventActionHandler handler, IEventInput input, IEventOutput output)
        {
            string token = null;
            input.Properties?.TryGetValue("token", out token);
            var user = JWTHelper.Default.GetUserInfo(token);
            if (user!=null)
            {
                return base.Executing(center, handler, input, output);
            }
            else
            {
                output.EventError = EventError.InnerError;
                output.Data = new object[] { "操作错误，无权操作相关资源！" };
                return false;
            }
        }
    }

}
