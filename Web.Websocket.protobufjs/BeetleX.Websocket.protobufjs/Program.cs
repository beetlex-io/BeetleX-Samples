using BeetleX.FastHttpApi.Hosting;
using BeetleX.FastHttpApi.WebSockets;
using BeetleX.Websocket.Protobuf;
using ProtoBuf;
using System;

namespace BeetleX.Websocket.protobufjs
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer host = new HttpServer(80);
            host.Setting((service, option) =>
            {
                option.LogToConsole = true;
                option.LogLevel = BeetleX.EventArgs.LogType.Info;

                option.SetDebug();
            });

            host.RegisterComponent<Program>();
            host.RegisterComponent<BeetleX.Websocket.Protobuf.ProtobufFormater>();
            host.Completed(http =>
            {
                http.AddExts("proto");
                http.WebSocketReceive = (r, e) =>
                {
                    if (e.Frame.Body is IProtobufCommand cmd)
                    {
                        cmd.Execute(e);
                    }
                };
            });
            host.Run();
        }
    }
    [ProtobufType(2)]
    [ProtoContract]
    public class Register : IProtobufCommand
    {
        [ProtoMember(2)]
        public string Password { get; set; }
        [ProtoMember(1)]
        public string EMail { get; set; }
        public void Execute(WebSocketReceiveArgs e)
        {
           
        }
    }

    [ProtobufType(1)]
    [ProtoContract]
    public class User : IProtobufCommand
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public string Email { get; set; }

        [ProtoMember(3)]
        public double ResponseTime { get; set; }

        public void Execute(WebSocketReceiveArgs e)
        {
            ResponseTime = DateTime.Now.Ticks;
            e.ResponseBinary(this);
        }
    }
}
