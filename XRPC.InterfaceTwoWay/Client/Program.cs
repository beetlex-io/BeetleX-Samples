using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program : IUser
    {
        static XRPCClient client;
        static void Main(string[] args)
        {
            client = new XRPCClient("192.168.2.18", 9090);
            client.PingTime = 5;
            client.Options.ParameterFormater = new JsonPacket();
            client.Register<IUser>(new Program());
            var user = client.Create<IUser>();
            user.Login("henry");
            System.Threading.Thread.Sleep(-1);
        }


        public Task<DateTime> GetTime()
        {
            return Task.FromResult(DateTime.Now);
        }

        public Task Login(string name)
        {
            return Task.CompletedTask;
        }
    }

    public interface IUser
    {
        Task Login(string name);

        Task<DateTime> GetTime();
    }
}
