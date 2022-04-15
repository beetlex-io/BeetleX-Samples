using CodeBenchmark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.CodeBenchmark.Sample
{
    [System.ComponentModel.Category("REDIS")]
    public class RedisIncrby : IExample
    {
        public void Dispose()
        {
           
        }

        public async Task Execute()
        {
            await Redis.DefaultRedis.Incrby("number", 10);
        }

        public void Initialize(Benchmark benchmark)
        {
            
        }
    }
}
