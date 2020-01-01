using System;
using System.Threading.Tasks;

namespace EventNext.Hello
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
            var hello = EventCenter.Default.Create<IHello>();
            while(true)
            {
                Console.Write("Enter name:");
                var name = Console.ReadLine();
                var result = await hello.Say(name);
                Console.WriteLine(result);
            }
        }
    }
    [Service(typeof(IHello))]
    public class HelloImpl : IHello
    {
        public Task<string> Say(string name)
        {
            return $"hello {name} {DateTime.Now}".ToTask();
        }
    }


    public interface IHello
    {
        Task<string> Say(string name);
    }

}
