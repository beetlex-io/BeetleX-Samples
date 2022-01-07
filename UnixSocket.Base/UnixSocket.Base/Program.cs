using BeetleX;
using BeetleX.Clients;
using BeetleX.EventArgs;
using System;
using System.Threading.Tasks;

namespace UnixSocket.Base
{
    class Program : BeetleX.ServerHandlerBase
    {
        //static string mHost = "127.0.0.1";

        static string mHost = "/tmp/beetlex_unixsocket.sock";

        static BeetleX.IServer mServer;

        static AsyncTcpClient mClient;
        static async Task Main(string[] args)
        {
            mServer = BeetleX.SocketFactory.CreateTcpServer<Program>();
            mServer.Options.DefaultListen.Host = mHost;
            mServer.Options.DefaultListen.Port = 9090;
            mServer.Open();
            mClient = SocketFactory.CreateClient<AsyncTcpClient>(mHost, 9090);
            mClient.DataReceive = (c, e) =>
            {
                if (e.Stream.ToPipeStream().TryReadLine(out string line))
                {
                    Console.WriteLine("receive:" + line); 
                    Enter();
                }
            };
            Enter();
            await Task.Delay(-1);
        }
        static void Enter() {
            Console.Write("enter name:");
            var name = Console.ReadLine();
            mClient.Stream.ToPipeStream().WriteLine(name);
            mClient.Stream.Flush();
        }

        public override void SessionReceive(IServer server, SessionReceiveEventArgs e)
        {
            base.SessionReceive(server, e);
            var stream = e.Stream.ToPipeStream();
            if (stream.TryReadLine(out string line))
            {
                stream.WriteLine(line);
                stream.Flush();

            }
        }
    }
}
