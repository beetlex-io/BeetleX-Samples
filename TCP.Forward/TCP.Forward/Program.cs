using System;
using System.Threading.Tasks;

namespace TCP.Forward
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new ForwardServer(null, 8080);
            await server.Push("192.168.1.128", 8080);
            await server.Push("192.168.1.129", 8080);
            server.Run();
            await Task.Delay(-1);
        }
    }
}
