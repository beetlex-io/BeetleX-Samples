using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static XRPCClient client;
        static IProductStorage henry, ken;
        static void Main(string[] args)
        {
            client = new XRPCClient("localhost", 9090);
            client.Options.ParameterFormater = new JsonPacket();//default messagepack
            henry = client.Create<IProductStorage>("henry");
            ken = client.Create<IProductStorage>("ken");
            Test();
            Console.Read();
        }

        static async void Test()
        {
            await Income();
            await Pay();
            Console.WriteLine($"henry actor:{await henry.GetValue()} | ken actor:{await ken.GetValue()}");
        }

        static async Task Income()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var t = Task.Run(async () =>
                {
                    for (int k = 0; k < 100; k++)
                    {
                        await henry.Put(10);
                        await ken.Put(10);

                    }
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks.ToArray());

        }

        static async Task Pay()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var t = Task.Run(async () =>
                {
                    for (int k = 0; k < 100; k++)
                    {
                        await henry.Out(10);
                        await ken.Out(10);

                    }
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }
    }

    public interface IProductStorage
    {
        Task Put(int quantity);
        Task Out(int quantity);
        Task<int> GetValue();
    }
}
