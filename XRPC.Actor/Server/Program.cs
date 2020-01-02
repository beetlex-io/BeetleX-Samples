using BeetleX.XRPC.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using EventNext;
using BeetleX.XRPC.Packets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
           .ConfigureServices((hostContext, services) =>
           {
               services.UseXRPC(s =>
               {
                   s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Warring;
                   s.ServerOptions.DefaultListen.Port = 9090;
                   s.RPCOptions.ParameterFormater = new JsonPacket();//default messagepack
               },
               typeof(Program).Assembly);
           });
            builder.Build().Run();
        }
    }

    public interface IAmount
    {
        Task Income(int value);
        Task Pay(int value);
        Task<int> GetValue();
    }
    [Service(typeof(IAmount))]
    public class AmountImpl : IAmount
    {

        private int mAmount;

        public Task<int> GetValue()
        {
            return mAmount.ToTask();
        }

        public Task Income(int value)
        {
            mAmount -= value;
            return Task.CompletedTask;
        }

        public Task Pay(int value)
        {
            mAmount += value;
            return Task.CompletedTask;
        }
    }


}
