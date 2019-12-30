using System;
using System.Threading.Tasks;
using BeetleX.Redis;
using System.Collections.Generic;
namespace EventNext.ThreadContainer
{
    class Program
    {
        static EventNext.EventCenter EventCenter = new EventCenter();
        static void Main(string[] args)
        {
            EventCenter.Register(typeof(Program).Assembly);
            Test();
            System.Threading.Thread.Sleep(-1);
        }

        static async void Test()
        {
            Console.WriteLine("Warm-Up...");
            await None(10);
            await Threads(1, 10);
            await Threads(2, 10);
            await Threads(4, 10);
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Test...");
                await None(2000);
                await Threads(1, 2000);
                await Threads(2, 2000);
                await Threads(4, 2000);
            }
        }

        static async Task None(int count)
        {
            var account = EventCenter.Create<IAccount>();
            List<Task> tasks = new List<Task>();
            var now = BeetleX.TimeWatch.GetElapsedMilliseconds();
            for (int i = 0; i < 20; i++)
            {
                var t = Task.Run(async () =>
                {
                    for (int k = 0; k < count; k++)
                        await account.Income("ken", k);
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
            var value = await account.Value("ken");
            Console.WriteLine($"none use time:{BeetleX.TimeWatch.GetElapsedMilliseconds() - now} ms");
        }

        static async Task Threads(int threads, int count)
        {
            var tc = EventCenter.GetThreadContainer($"t{threads}", threads);
            var account = tc.Create<IAccount>();
            List<Task> tasks = new List<Task>();
            var now = BeetleX.TimeWatch.GetElapsedMilliseconds();
            for (int i = 0; i < 20; i++)
            {
                var t = Task.Run(async () =>
                {
                    for (int k = 0; k < count; k++)
                        await account.Income("ken", k);
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
            var value = await account.Value("ken");
            Console.WriteLine($"{threads} thread use time:{BeetleX.TimeWatch.GetElapsedMilliseconds() - now} ms");
        }


    }

    [Service(typeof(IAccount))]
    public class AccountImpl : IAccount
    {

        static AccountImpl()
        {
            Redis.Default.Host.AddWriteHost("192.168.2.19");
        }

        public async Task Income(string name, int value)
        {
            await Redis.Default.Incrby(name, value);
        }

        public async Task<string> Value(string name)
        {
            return await Redis.Default.Get<string>(name);
        }
    }


    public interface IAccount
    {
        Task<string> Value(string name);

        Task Income(string name, int value);

    }


}
