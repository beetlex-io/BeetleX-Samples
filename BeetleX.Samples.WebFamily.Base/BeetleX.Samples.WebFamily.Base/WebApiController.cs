using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using Microsoft.EntityFrameworkCore;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Samples.WebFamily.Base
{
    [Controller]
    [AuthMark(AuthMarkType.NoValidation)]
    public class WebApiController
    {
        public object Hello()
        {
            return DateTime.Now;
        }
        public DBObjectList<Customer> Customers(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select * from customers");
        }

        public async Task SetRedis(EFCoreDB<NorthwindContext> db, NorthwindRedis redis)
        {
            var item = await db.DBContext.Customers.FirstAsync();
            await redis.Set(item.CustomerID, item);
        }

        public async Task<Customer> GetRedis(NorthwindRedis redis)
        {
            var item = await redis.Get<Customer>("ALFKI");
            return item;
        }
    }
}
