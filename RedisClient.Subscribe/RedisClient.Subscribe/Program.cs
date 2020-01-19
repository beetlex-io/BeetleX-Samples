using BeetleX.Redis;
using Northwind.Data;
using System;

namespace RedisClient.Subscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            Redis.Default.DataFormater = new ProtobufFormater();
            Redis.Default.Host.AddWriteHost("192.168.2.19");
            var subscribe = Redis.Default.Subscribe();
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
                await Redis.Publish("employee_queue", DataHelper.Defalut.Employees[index % DataHelper.Defalut.Employees.Count]);
                System.Threading.Thread.Sleep(1000);
                await Redis.Publish("customer_queue", DataHelper.Defalut.Customers[index % DataHelper.Defalut.Customers.Count]);
                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}
