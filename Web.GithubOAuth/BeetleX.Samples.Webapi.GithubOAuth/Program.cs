using BeetleX.WebFamily;
using System;

namespace BeetleX.Samples.Webapi.GithubOAuth
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            host.Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Info;
                o.LogToConsole = true;
            })   
            .RegisterComponent<Program>()
            .Run();
        }
    }
}
