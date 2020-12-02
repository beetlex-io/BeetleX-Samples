using BeetleX.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Samples.WebFamily.Base
{
    public class NorthwindRedis : RedisDB
    {
        public NorthwindRedis() : base(0, new JsonFormater())
        {

        }
    }
}
