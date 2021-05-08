using BeetleX.Buffers;
using BeetleX.XRPC.Clients;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {

        static XRPCClient client;
        static IUserService UserService;
        static long mCount;
        static long mLastCount;
        static int[] mUsers = new int[] { 50, 100, 200, 500 };
        static int mRequests = 1000000;

        static void Main(string[] args)
        {
            BufferPool.BUFFER_SIZE = 1024 * 32;
            client = new XRPCClient("192.168.2.19", 9090, 1);
            client.NetError = (c, e) =>
            {
                Console.WriteLine(e.Error.Message);
            };
            client.TimeOut = 10000;
            UserService = client.Create<IUserService>();
            int thread = 1;
            if (args != null && args.Length > 0)
                thread = int.Parse(args[0]);
            Test(thread);
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine($"{DateTime.Now} {mCount:000,000,000,000}/{mCount - mLastCount:###,###,###}");
                mLastCount = mCount;
            }
        }
        static void Test(int threads)
        {
            if (threads < 1)
                threads = 1;
            for (int i = 0; i < threads; i++)
            {
                MutilTest();
            }
        }

        static async Task MutilTest()
        {
            while (true)
            {
                await UserService.List(1);
                System.Threading.Interlocked.Increment(ref mCount);
            }
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
