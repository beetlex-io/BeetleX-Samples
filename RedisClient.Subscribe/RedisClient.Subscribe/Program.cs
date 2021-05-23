using BeetleX.Redis;
using Northwind.Data;
using System;

namespace RedisClient.Subscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            DefaultRedis.Instance.DataFormater = new ProtobufFormater();
            DefaultRedis.Instance.Host.AddWriteHost("127.0.0.1");
            var subscribe = DefaultRedis.Instance.Subscribe();
            subscribe.Register<Employee>("employee_queue", e =>
            {
                Console.WriteLine($"Receive employee {e.FirstName} {e.LastName}");
            });
            subscribe.Register<Customer>("customer_queue", e =>
            {
                Console.WriteLine($"Receive customer {e.CustomerID} {e.CompanyName}");
            });
            subscribe.Listen();
            Publish();
            Console.Read();
        }
        static async void Publish()
        {
            Random ram = new Random();
            while (true)
            {
                int index = ram.Next(0, 1000000);
                await DefaultRedis.Instance.Publish("employee_queue", DataHelper.Defalut.Employees[index % DataHelper.Defalut.Employees.Count]);
                System.Threading.Thread.Sleep(1000);
                await DefaultRedis.Instance.Publish("customer_queue", DataHelper.Defalut.Customers[index % DataHelper.Defalut.Customers.Count]);
                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}
