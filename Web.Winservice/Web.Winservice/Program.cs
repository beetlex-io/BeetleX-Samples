using System;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
namespace Web.Winservice
{
    class Program
    {
        private static HttpServer mServer;

        static void Main(string[] args)
        {
            mServer = new HttpServer(80);
            mServer.IsWindowsServices = true;
            mServer.Setting((service, option) =>
            {
                option.LogToConsole = true;
                option.WriteLog = true;
                option.LogLevel = BeetleX.EventArgs.LogType.Info;
            });
            mServer.RegisterComponent<Home>();
            mServer.Run();
        }
    }
    [Controller]
    public class Home
    {
        public object Hello(string name)
        {
            return $"hello {name}";
        }
    }
}
