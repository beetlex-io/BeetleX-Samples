using System;
using BeetleX;
using BeetleX.Buffers;
using BeetleX.EventArgs;

namespace Server
{
    public class Program : IApplication
    {
        private static ServerBuilder<Program, User, Messages.JsonPacket> server;
        public static void Main(string[] args)
        {

            server = new ServerBuilder<Program, User, Messages.JsonPacket>();
            server.ConsoleOutputLog = true;
            server.SetOptions(option =>
            {
                option.DefaultListen.Port = 9090;
                option.DefaultListen.Host = "127.0.0.1";
                option.LogLevel = LogType.Trace;
            })
            .OnMessageReceive<Messages.Register>((e) =>
            {
                Console.WriteLine($"application:{e.Application}\t session:{e.Session}");
                e.Message.DateTime = DateTime.Now;
                e.Return(e.Message);
            })
            .OnMessageReceive((e) =>
            {

            })
            .Run();
            Console.Read();
        }

        public void Init(IServer server)
        {
            Console.WriteLine("application init");
        }
    }

    public class User : ISessionToken
    {
        public void Dispose()
        {
            Console.WriteLine("client disposed");
        }

        public void Init(IServer server, ISession session)
        {
            Console.WriteLine("session init");
        }
    }
}
