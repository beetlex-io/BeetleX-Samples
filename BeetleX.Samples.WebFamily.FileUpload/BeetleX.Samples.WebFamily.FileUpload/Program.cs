using BeetleX.WebFamily;
using System;

namespace BeetleX.Samples.WebFamily.FileUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            WebHost.HomeModel = "webfamily-files";
            

            host.Setting(o =>
            {

                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Info;
                o.LogToConsole = true;
            })
                .UseFileManager(o =>
                {
                    o.MaxSize = 1024 * 1024 * 1024;
                })
            .UseElement(PageStyle.Element)
            .Initialize((http, vue, resoure) =>
            {
                vue.Debug();
            }).Run();
        }
    }
}

