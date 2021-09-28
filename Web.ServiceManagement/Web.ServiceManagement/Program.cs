using BeetleX.EventArgs;
using BeetleX.WebFamily;
using System;

namespace Web.ServiceManagement
{
    class Program
    {
        static void Main(string[] args)
        {

            WebHost host = new WebHost();
            host.IsWindowsServices = true;
            WebHost.Title = "Service Management";
            WebHost.HeaderModel = "beetlex-process-header";
            WebHost.HomeModel = "beetlex-process-home";
            WebHost.TabsEnabled = false;
            host.RegisterComponent<Program>();
            host.RegisterComponent<BeetleX.ServicesProcess.ProcessCenter>();
            host.UseFontawesome();
            host.UseElement(PageStyle.Element);
            host.Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = LogType.Info;
            });
            host.Initialize((http, vue, rec) =>
            {
                BeetleX.ServicesProcess.WebController controller = new BeetleX.ServicesProcess.WebController();
                controller.Init(new logHandler(http));
                http.ActionFactory.Register(controller, new BeetleX.FastHttpApi.ControllerAttribute { BaseUrl = "process" });
                rec.AddCss("website.css");
                vue.Debug();
            });
            host.Run();

        }
    }
    class logHandler : BeetleX.ServicesProcess.ILogHandler
    {
        public logHandler(BeetleX.FastHttpApi.HttpApiServer sever)
        {
            mServer = sever;
        }


        private BeetleX.FastHttpApi.HttpApiServer mServer;

        public void Error(string message)
        {
            mServer.GetLog(LogType.Error)?.Log(LogType.Error, null, message);
        }

        public void Info(string message)
        {
            mServer.GetLog(LogType.Info)?.Log(LogType.Info, null, message);
        }
    }

}
