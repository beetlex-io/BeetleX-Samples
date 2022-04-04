using System;
using BeetleX.FastHttpApi.Hosting;
namespace Websocket.Authorization
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer(80);
            server.Setting((service, option) =>
            {
                option.LogToConsole = true;
                option.LogLevel = BeetleX.EventArgs.LogType.Trace;
            }).Completed(http =>
            {
                http.WebSocketConnect += (o, e) =>
                {
                    if (e.Request.Authorization != "beetlex")
                    {
                        e.Cancel = true;
                        e.Error = new BeetleX.FastHttpApi.UpgradeWebsocketError(500, "Authorization error!");
                    }
                };
                http.WebSocketReceive = (o, e) =>
                {
                    e.ResponseText("beetlex websocket");
                };
            }).Run();
        }
    }
}
