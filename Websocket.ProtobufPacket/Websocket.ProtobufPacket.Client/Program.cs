using System;
using System.Threading.Tasks;

namespace Websocket.ProtobufPacket.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ProtobufClient.BinaryDataFactory.RegisterComponent<User>();
            var client = new ProtobufClient("ws://localhost");
            User user = new User { Email="henryfan@msn.com", Name="henryfan" };
            var result = await client.ReceiveFrom<User>(user);
            Console.WriteLine(result.Name);
        }
    }
}
