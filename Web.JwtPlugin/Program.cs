using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using BeetleX.FastHttpApi.Jwt;
using Microsoft.Extensions.Hosting;
using System;

namespace Web.JwtPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
             .ConfigureServices((hostContext, services) =>
             {
                 //BeetleX.FastHttpApi.Hosting
                 services.UseBeetlexHttp(o =>
                 {
                     o.Port = 80;
                     o.LogToConsole = true;
                 },
                 http =>
                 {
                     http.UseJWT();//BeetleX.FastHttpApi.Jwt
                 },
                 typeof(Program).Assembly);
             });
            builder.Build().Run();
        }
    }
    [Controller]
    public class Home
    {
        
        public Object GetTime()
        {
            return DateTime.Now;
        }
        [AuthMark(AuthMarkType.NoValidation)]
        public void Login(string name,string pwd,IHttpContext context)
        {
            context.SetJwtToken(name, "admin");
        }
    }

}
