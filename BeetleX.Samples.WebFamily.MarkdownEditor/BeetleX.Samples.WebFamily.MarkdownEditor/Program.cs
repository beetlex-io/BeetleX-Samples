using BeetleX.WebFamily;
using System;

namespace BeetleX.Samples.WebFamily.MarkdownEditor
{
    class Program
    {
        static void Main(string[] args)
        {


            WebHost host = new WebHost();
            WebHost.Title = "Beetlex WebFamily";
            WebHost.HomeModel = "webfamily-md";
            WebHost.HomeName = "Markdown";
            WebHost.Login = false;
            host.RegisterComponent<Program>();
            host.UseToastUIEditor();
            host.Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Info;
                o.LogToConsole = true;
            })
            .UseElement(PageStyle.Element)
            .Initialize((http, vue, resoure) =>
            {
                resoure.AddCss("website.css");
                vue.Debug();
            }).Run();
        }
    }
}
