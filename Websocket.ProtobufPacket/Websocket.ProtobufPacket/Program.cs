using System;

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
                http.WebSocketReceive = (o, e) =>
                {
                    var user = (User)e.Frame.Body;
                    user.ResultTime = DateTime.Now;
                    e.ResponseBinary(user);
                };
            });
            server.Run();
        }
    }
}
