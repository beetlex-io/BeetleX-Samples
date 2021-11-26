using BeetleX.Http.Clients;
using System;
using System.Threading.Tasks;

namespace CodeBenchmark.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Benchmark benchmark = new Benchmark();
            benchmark.Register(typeof(Program).Assembly);
            benchmark.Start(80,true);
            
            Console.Read();
        }
    }
    [System.ComponentModel.Category("HTTP")]
    class BaiduGet : IExample
    {
        public void Dispose()
        {
        }

        public async Task Execute()
        {
            var result = await "https://www.baidu.com".FormUrlRequest().Get();
        }

        public void Initialize(Benchmark benchmark)
        {

        }


    }

    [System.ComponentModel.Category("HTTP")]
    class WYGet : IExample
    {
        public void Dispose()
        {
        }

        public async Task Execute()
        {
            var result = await "https://www.163.com".FormUrlRequest().Get();
        }

        public void Initialize(Benchmark benchmark)
        {

        }


    }
}
