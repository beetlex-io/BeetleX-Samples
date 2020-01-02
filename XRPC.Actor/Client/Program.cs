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
        static IAmount henry, ken, none;
        static void Main(string[] args)
        {
            client = new XRPCClient("localhost", 9090);
            client.Options.ParameterFormater = new JsonPacket();//default messagepack
            henry = client.Create<IAmount>("henry");
            ken = client.Create<IAmount>("ken");
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
                        await henry.Income(10);
                        await ken.Income(10);

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
                       await henry.Pay(10);
                        await ken.Pay(10);

                    }
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }
    }

    public interface IAmount
    {
        Task Income(int value);
        Task Pay(int value);
        Task<int> GetValue();
    }
}
