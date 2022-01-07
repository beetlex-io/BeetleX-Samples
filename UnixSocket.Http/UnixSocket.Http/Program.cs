using System;
using System.Threading.Tasks;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using BeetleX.Http.Clients;

namespace UnixSocket.Http
{
    [BeetleX.FastHttpApi.Controller]
    public class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer(8080);
            server.RegisterComponent<Program>();
            server.Setting((service, o) =>
            {
                o.LogToConsole = true;
                o.SockFile = "/tmp/beetlex_http.sock";
            });
            server.Completed(http =>
            {
                Task.Run(Test);
            });
            server.Run();
        }
        static async Task Test()
        {
            var request = new HttpFormUrlClient("http://localhost:8080/GetTime");
            var response = await request.Get();
            Console.WriteLine("http socket response:" + response.Body);

            request = new HttpFormUrlClient("/tmp/beetlex_http.sock/GetTime");
            response = await request.Get();
            Console.WriteLine("http unix socket response:" + response.Body);

        }

        public object GetTime()
        {
            return new TextResult(DateTime.Now.ToString());
        }


    }
}
