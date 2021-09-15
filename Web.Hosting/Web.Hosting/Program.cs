using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Web.Hosting
{

    public class Program
    {


        static void Main(string[] args)
        {
            HttpServer host = new HttpServer(80);
            host.UseTLS("test.pfx", "123456");
            host.Setting((service, option) =>
            {
                service.AddTransient<UserInfo>();
                option.LogToConsole = true;
                option.LogLevel = BeetleX.EventArgs.LogType.Info;
            });
            host.RegisterComponent<Program>();
            host.Run();
        }
    }
    [Controller]
    public class Home
    {
        public Home(UserInfo user)
        {
            mUser = user;
        }

        public object Hello()
        {
            return mUser.Name;
        }

        private UserInfo mUser;
    }

    public class UserInfo
    {
        public string Name { get; set; } = "admin";
    }
}
