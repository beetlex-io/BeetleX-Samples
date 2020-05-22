using BeetleX.FastHttpApi.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Web.Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] obj = Environment.GetCommandLineArgs();
            var builder = new HostBuilder()
             .ConfigureServices((hostContext, services) =>
             {
                 services
                 .UseBeetlexHttp(o =>
                 {
                     o.Port = 80;
                     o.LogToConsole = true;
                     o.LogLevel = BeetleX.EventArgs.LogType.Info;
                     o.SetDebug();
                 },
                 HttpServer =>
                 {

                 },
                 typeof(Program).Assembly, typeof(BeetleX.FastHttpApi.Admin._Admin).Assembly);
             });
            builder.Build().Run();
        }
    }
}
