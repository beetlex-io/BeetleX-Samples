using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static XRPCClient client;
        static IDataService dataService;
        static void Main(string[] args)
        {
            client = new XRPCClient("localhost", 9090,"test");
            client.CertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
            client.Options.ParameterFormater = new JsonPacket();//default messagepack
            dataService = client.Create<IDataService>();
            Test();
            Console.Read();
        }

        static async void Test()
        {
            try
            {
                await dataService.List();
            }
            catch (Exception e_)
            {
                Console.WriteLine(e_.Message);
            }
            var token = await dataService.Login("admin", "123456");
            ((EventNext.IHeader)dataService).Header["token"] = token;
            Console.WriteLine($"token:{token}");
            var items = await dataService.List();
            foreach (var item in items)
            {
                Console.WriteLine($"{item.CompanyName}\t{item.ContactName}");
            }
        }
    }

    public interface IDataService
    {
        Task<string> Login(string name, string pwd);

        Task<List<Customer>> List();
    }
}
