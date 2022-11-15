using System;
using System.Net;
using System.Threading.Tasks;
using BeetleX;
using BeetleX.Buffers;
using BeetleX.Clients;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
var client = new AwaiterClient("127.0.0.1", 9090, new Messages.JsonClientPacket());
while (true)
{
    Messages.Register register = new Messages.Register();
    Console.Write("Enter Name:");
    register.Name = Console.ReadLine();
    Console.Write("Enter Email:");
    register.EMail = Console.ReadLine();
    Console.Write("Enter City:");
    register.City = Console.ReadLine();
    Console.Write("Enter Password:");
    register.PassWord = Console.ReadLine();
    await client.Send(register);
    var result = await client.Receive<Messages.Register>();
    Console.WriteLine($"{result.Name} {result.EMail} {result.City} {result.DateTime}");
}
        }
    }


}
