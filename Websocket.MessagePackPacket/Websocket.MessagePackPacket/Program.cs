using BeetleX.FastHttpApi.WebSockets;
using System;

namespace Websocket.MessagePackPacket
{
    class Program
    {
        static void Main(string[] args)
        {
            BeetleX.FastHttpApi.Hosting.HttpServer server = new BeetleX.FastHttpApi.Hosting.HttpServer(80);
            server.Setting((service, options) =>
            {
                options.LogLevel = BeetleX.EventArgs.LogType.Trace;
                options.LogToConsole = true;
                options.ManageApiEnabled = false;
                options.WebSocketFrameSerializer = new MessagePackFrameSerializer();
            });
            server.Completed(http =>
            {
                http.RegisterMessagePack<User>();
                http.UseMessagePackController();
            });
            server.Run();
        }
    }

    [MessageController]
    public class Controller
    {
        public User Login(WebSocketReceiveArgs e, User user)
        {
            user.ResultTime = DateTime.Now;
            return user;
        }
    }
}
