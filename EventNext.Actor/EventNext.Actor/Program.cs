using System;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace EventNext.Actor
{
    class Program
    {
        static void Main(string[] args)
        {
            EventCenter.Default.Register(typeof(Program).Assembly);
            Test();
            System.Threading.Thread.Sleep(-1);
        }

        static async void Test()
        {
            var all = EventCenter.Default.Create<IAccount>();
            //var all = EventCenter.Default.GetThreadContainer("t1").Create<IAccount>();
            var henry = EventCenter.Default.Create<IAccount>("henry");
            var ken = EventCenter.Default.Create<IAccount>("ken");
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var t = Task.Run(async () =>
                {
                    for (int k = 0; k < 100; k++)
                    {
                        await all.Income(k);
                        await henry.Income(k);
                        await ken.Income(k);
                    }
                });
                tasks.Add(t);

                t = Task.Run(async () =>
                {
                    for (int k = 0; k < 100; k++)
                    {
                        await all.Pay(k);
                        await henry.Pay(k);
                        await ken.Pay(k);
                    }
                });
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
            Console.WriteLine($"all:{await all.Value()}");
            Console.WriteLine($"ken:{await ken.Value()}");
            Console.WriteLine($"henry:{await henry.Value()}");
        }
    }

    [Service(typeof(IAccount))]
    public class AccountImpl : IAccount
    {

        private long mValue;

        public Task Income(int value)
        {
            mValue += value;
            return Task.CompletedTask;
        }

        public Task Pay(int value)
        {
            mValue -= value;
            return Task.CompletedTask;
        }

        public Task<long> Value()
        {
            return mValue.ToTask();
        }
    }


    public interface IAccount
    {
        Task Income(int value);

        Task Pay(int value);

        Task<long> Value();
    }


}
