using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace WebSocket.Hello
{
    class Program
    {



        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.UseBeetlexHttp(o =>
                    {
                        o.LogToConsole = true;
                        o.ManageApiEnabled = false;
                        o.Port = 80;
                        o.SetDebug();
                        o.LogLevel = BeetleX.EventArgs.LogType.Warring;
                    }, http =>
                    {
                        http.WebSocketConnect = (o, e) =>
                        {
                            //e.Cancel = true;
                            //e.Error = new UpgradeWebsocketError(400, "websocket error!");
                        };
                    },
                    typeof(Program).Assembly);
                });
            builder.Build().Run();
        }
    }
    [BeetleX.FastHttpApi.Controller]
    public class Home : BeetleX.FastHttpApi.IController
    {
        public string Hello(string name)
        {
            return $"hello {name} {DateTime.Now}";
        }

        private HttpApiServer mServer;

        [NotAction]
        public void Init(HttpApiServer server, string path)
        {
            mServer = server;
            Task.Run(() =>
            {
                while (true)
                {
                    if (this.mServer.BaseServer != null)
                    {
                        var items = this.mServer.GetWebSockets();
                        Console.WriteLine($"WS-Online:{items.Count}");
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            });
        }
    }
}
