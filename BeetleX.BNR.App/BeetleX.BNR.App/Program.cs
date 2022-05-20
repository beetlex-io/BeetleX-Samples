using BeetleX.Redis;
using System;
using System.Threading.Tasks;

namespace BeetleX.BNR.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DefaultRedis.Instance.Host.AddWriteHost("localhost");
            BNRFactory.Default.Register("redis", new RedisSequenceParameter(DefaultRedis.Instance));
            string[] citys = new string[] { "广州", "深圳", "上海", "北京" };
            foreach (var item in citys)
            {
                Console.WriteLine(await BNRFactory.Default.Create($"[CN:{item}][N:[CN:{item}]/0000000]"));
            }

            foreach (var item in citys)
            {
                Console.WriteLine(await BNRFactory.Default.Create($"[CN:{item}][redis:city/0000000]"));
            }
            Console.Read();

        }
    }
}
