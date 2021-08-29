using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using System;

namespace BeetleX.Samples.Webapi.EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpApiServer server = new HttpApiServer();
            server.AddEFCoreDB<NorthwindEFCoreSqlite.NorthwindContext>();
            server.Options.Port = 80;
            server.Options.LogToConsole = true;
            server.Options.LogLevel = EventArgs.LogType.Info;
            server.Options.SetDebug();
            server.Register(typeof(Program).Assembly);
            server.AddExts("woff");
            server.Open();
            Console.Read();
        }
    }
}
