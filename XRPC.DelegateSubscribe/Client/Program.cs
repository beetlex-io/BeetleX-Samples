using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using Northwind.Data;
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
client.Subscribe<Action<DateTime>>(d =>
{
    Console.WriteLine($"Subscribe DateTime {d}");
});
client.Subscribe<Action<Order>>(d =>
{
    Console.WriteLine($"Subscribe Order {d.OrderID} {d.OrderDate}");
});
            System.Threading.Thread.Sleep(-1);
        }
    }
}
