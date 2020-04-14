using System;

namespace Websocket.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();

            Console.Read();
        }

        static async void Test()
        {
            var ws = new BeetleX.Http.WebSockets.TextClient("ws://echo.websocket.org");

            var result = await ws.ReceiveFrom("ws hello henry");
            Console.WriteLine(result);

            var wss = new BeetleX.Http.WebSockets.TextClient("wss://echo.websocket.org");

            result = await wss.ReceiveFrom("wss hello henry");
            Console.WriteLine(result);
        }
    }
}
