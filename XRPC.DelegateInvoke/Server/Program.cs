using BeetleX.EventArgs;
using BeetleX.XRPC;
using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using Microsoft.Extensions.Hosting;
using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static XRPCServer mXRPCServer;

        static void Main(string[] args)
        {
            var builder = new HostBuilder()
              .ConfigureServices((hostContext, services) =>
              {
                  services.UseXRPC(s =>
                  {
                      s.ServerOptions.LogLevel = LogType.Warring;
                      s.RPCOptions.ParameterFormater = new JsonPacket();
                  }, c =>
                  {
                      c.AddDelegate<ListEmployees>(() => Task.FromResult(DataHelper.Defalut.Employees));
                      c.AddDelegate<ListCustomers>(() => Task.FromResult(DataHelper.Defalut.Customers));
                      c.AddDelegate<ListOrders>((emp, cust) =>
                      {
                          Func<Order, bool> filter = (o) => (emp == 0 || o.EmployeeID == emp) && (String.IsNullOrEmpty(cust) || o.CustomerID == cust);
                          return Task.FromResult((from a in DataHelper.Defalut.Orders where filter(a) select a).ToList());
                      });
                      Task.Run(() => {
                          while(true)
                          {
                              foreach (var item in c.Server.GetOnlines())
                              {

                                  c.Delegate<Action<DateTime>>(item)(DateTime.Now);
                                  System.Threading.Thread.Sleep(1000);
                              }
                          }
                      });

                  },
                  typeof(Program).Assembly);
              });
            builder.Build().Run();
        }
    }

    public delegate Task<List<Order>> ListOrders(int employee, string employeeid);

    public delegate Task<List<Employee>> ListEmployees();

    public delegate Task<List<Customer>> ListCustomers();
}
