using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using System;

namespace Client
{
    class Program
    {
        static XRPCClient client;
        static void Main(string[] args)
        {
            client = new XRPCClient("192.168.1.18", 9090);
            //client.PingTimeout = 5;
            client.Options.ParameterFormater = new JsonPacket();
          
            client.TimeOut = 100000;
            //  TestTaskResultDelegate(client);
            //UnitTest();
            var task = client.Subscribe<Action<DateTime>>(d =>
            {
                Console.WriteLine(d);
            });
            System.Threading.Thread.Sleep(-1);
        }
    }
}
