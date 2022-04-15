using CodeBenchmark;
using System;

namespace BeetleX.CodeBenchmark.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            BeetleX.Redis.DefaultRedis.Instance.Host.AddWriteHost("localhost");
            Benchmark benchmark = new Benchmark();
            benchmark.Register(typeof(Program).Assembly);
            benchmark.Start(80, true);
        }
    }
}
