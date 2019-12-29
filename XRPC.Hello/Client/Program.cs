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
        static void Main(string[] args)
        {
            client = new XRPCClient("localhost", 9090);
            client.Options.ParameterFormater = new JsonPacket();//default messagepack
            hello = client.Create<IHello>();
            while(true)
            {
                Console.Write("Enter you name:");
                var name = Console.ReadLine();
                var task = hello.Hello(name);
                task.Wait();
                Console.WriteLine(task.Result);
            }
            Console.Read();
        }
    }

    public interface IHello
    {
        Task<string> Hello(string name);
    }
}
