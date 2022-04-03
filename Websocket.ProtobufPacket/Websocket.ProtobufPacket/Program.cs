using BeetleX.FastHttpApi.WebSockets;
using System;
using System.Threading.Tasks;

namespace Websocket.ProtobufPacket
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
                options.WebSocketFrameSerializer = new ProtobufFrameSerializer();
            });
            server.Completed(http =>
            {
                http.RegisterProtobuf<User>();
                http.UseProtobufController();
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
