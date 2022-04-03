using System;
using System.Threading.Tasks;

namespace Websocket.MessagePackPacket.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MessagePackClient.BinaryDataFactory.RegisterComponent<User>();
            var client = new MessagePackClient("ws://localhost");
            User user = new User { Email = "henryfan@msn.com", Name = "henryfan" };
            var result = await client.ReceiveFrom<User>(user);
            Console.WriteLine(result.Name);
        }
    }
}
