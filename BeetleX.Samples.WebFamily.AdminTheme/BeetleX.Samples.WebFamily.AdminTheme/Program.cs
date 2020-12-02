using BeetleX.FastHttpApi.VueExtend;
using BeetleX.WebFamily;
using NorthwindEFCoreSqlite;
using System;

namespace BeetleX.Samples.WebFamily.AdminTheme
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            host.RegisterController<Program>()
            .Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Error;
                o.LogToConsole = true;
            })
            .UseEFCore<NorthwindContext>()
            .Initialize(s =>
            {
                s.GetWebFamily().AddScript("echarts.js");
                s.GetWebFamily().AddCss("website.css");
                s.Vue().Debug();
            }).Run();
        }
    }
}
