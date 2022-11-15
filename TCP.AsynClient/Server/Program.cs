using BeetleX;
using BeetleX.EventArgs;
using System;

namespace Server
{
    class Program : IApplication
    {
        private static ServerBuilder<Program, User> server;
        public static void Main(string[] args)
        {
            server = new ServerBuilder<Program, User>();
            server.SetOptions(option =>
            {
                option.DefaultListen.Port = 9090;
                option.DefaultListen.Host = "127.0.0.1";
            })
            .OnStreamReceive(e =>
            {
                Console.WriteLine($"session:{e.Session}\t application:{e.Application}");
                if (e.Reader.TryReadLine(out string name))
                {
                    Console.WriteLine(name);
                    e.Writer.WriteLine("hello " + name);
                    e.Flush();
                }
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

        }

        public void Init(IServer server, ISession session)
        {
            Console.WriteLine("session token init");
        }
    }
}
