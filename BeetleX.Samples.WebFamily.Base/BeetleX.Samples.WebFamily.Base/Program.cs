using BeetleX.FastHttpApi.VueExtend;
using BeetleX.WebFamily;
using NorthwindEFCoreSqlite;
using System;

namespace BeetleX.Samples.WebFamily.Base
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            host.RegisterController<WebApiController>()
            .Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Info;
                o.LogToConsole = true;
            })
            .UseEFCore<NorthwindContext>()
            .UseRedis<NorthwindRedis>(redis =>
            {
                var host = redis.Host.AddWriteHost("127.0.0.1");
                host.MaxConnections = 50;
            })
            .Initialize((http,vue,rec) =>
            {
                http.ResourceCenter.SetDefaultPages("index.html");
                rec.AddCss("site.css");
                vue.Debug();
            }).Run();
        }
    }
}
