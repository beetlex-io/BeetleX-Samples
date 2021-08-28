using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using BeetleX.FastHttpApi.Jwt;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Web.JWT
{

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.UseBeetlexHttp(o =>
                    {
                        o.LogToConsole = true;
                        o.LogLevel = BeetleX.EventArgs.LogType.Warring;
                        o.Port = 80;
                        o.SetDebug();
                    },
                    (http) =>
                    {
                        http.UseJWT((o, e) =>
                        {
                            var token = e.HttpContext.Data["token"];
                            if (token != null)
                            {
                                if (token == "admin")
                                {
                                    e.Success();
                                }
                                else
                                {
                                    e.Failure("当前凭证无效！");
                                }
                            }
                        });
                    },
                    typeof(Program).Assembly);
                });
            builder.Build().Run();
        }
    }
    [BeetleX.FastHttpApi.Controller]
    public class Home
    {
        [AuthMark(AuthMarkType.NoValidation)]
        public bool Login(string name, string pwd, IHttpContext context)
        {
            var result = (name == "admin" && pwd == "123456");
            if (result)
                context.SetAdminJwtToken(name);
            return result;
        }
        [AuthMark(AuthMarkType.Admin)]
        public object List()
        {
            return Northwind.Data.DataHelper.Defalut.Employees;
        }
    }
}
