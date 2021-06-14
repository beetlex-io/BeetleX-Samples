using BeetleX.WebFamily;
using System;

namespace BeetleX.Samples.WebFamily.ECharts
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHost();
            host.Setting(o =>
            {
                o.SetDebug();
                o.Port = host.AppConfig.Port;
                o.LogLevel = EventArgs.LogType.Error;
                o.WriteLog = true;
                o.LogToConsole = true;
            })
            .UseFontawesome()
            .Initialize((http, vue, rec) =>
            {
                rec.AddAssemblies(typeof(Program).Assembly);
                vue.Debug();
            }
               );
            host.Run();
        }
    }
}
