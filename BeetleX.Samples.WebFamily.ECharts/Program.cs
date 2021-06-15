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
            })
            .UseFontawesome()
            .Initialize((http, vue, rec) =>
            {
                rec.AddAssemblies(typeof(Program).Assembly);
                vue.Debug();
            });
            host.Run();
        }
    }
}
