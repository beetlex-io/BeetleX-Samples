using BeetleX.XRPC;
using BeetleX.XRPC.Hosting;
using EventNext;
using MessagePack;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestServer
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
                      s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Error;
                      s.ServerOptions.BufferSize = 1024 * 32;
                      s.Security = false;
                  }, c =>
                  {

                  },
                  typeof(Program).Assembly);
              });

            builder.Build().Run();
        }


    }

    [Service(typeof(IUserService))]
    public class UserService : IUserService
    {
        public UserService(XRPCServer server)
        {

        }

        public Task<List<User>> List(int count)
        {
            List<User> result = new List<User>();
            for (int i = 0; i < count; i++)
            {
                User user = new User();
                user.ID = Guid.NewGuid().ToString("N");
                user.City = "GuangZhou";
                user.EMail = "Henryfan@msn.com";
                user.Name = "henryfan";
                user.Remark = "http://ikende.com";
                result.Add(user);
            }
            return Task.FromResult(result);
        }

    }

    public interface IUserService
    {


        Task<List<User>> List(int count);
    }

    [MessagePackObject]
    public class User
    {
        [Key(4)]
        public string ID { get; set; }

        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string City { get; set; }

        [Key(2)]
        public string EMail { get; set; }

        [Key(3)]
        public string Remark { get; set; }
    }
}
