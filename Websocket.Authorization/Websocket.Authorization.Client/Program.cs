using BeetleX.Http.WebSockets;
using System;
using System.Threading.Tasks;

namespace Websocket.Authorization.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new TextClient("ws://localhost");
            client.Headers["Authorization"] = "beetlex";
            var result = await client.ReceiveFrom("beetlex");
            Console.WriteLine(result);
        }
    }
}
