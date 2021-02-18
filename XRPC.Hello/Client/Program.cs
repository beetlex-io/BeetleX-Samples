using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static XRPCClient client;
        static IHello hello;
        static async Task Main(string[] args)
        {
            client = new XRPCClient("localhost", 9090);
            client.Options.ParameterFormater = new JsonPacket();//default messagepack
            hello = client.Create<IHello>();
            while (true)
            {
                Console.Write("Enter you name:");
                var name = Console.ReadLine();
                var result = await hello.Hello(name);
                Console.WriteLine(result);
            }
            Console.Read();
        }
    }

    public interface IHello
    {
        Task<string> Hello(string name);
    }
}
