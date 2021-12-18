using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi.EFCore.Extension;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.WebFamily.Base
{
    [BeetleX.FastHttpApi.Controller]
    public class Home
    {
        public DBObjectList<ExpandoObject> CustomerCountrySelectOptions(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select DISTINCT country value from customers");
        }
        public object Customers(string country, string name, int index, EFCoreDB<NorthwindContext> db)
        {
            SQL sql = "select * from customers where 1=1";
            if (!string.IsNullOrEmpty(country))
                sql.And().Where<Customer>(c => c.Country == country);
            if (!string.IsNullOrEmpty(name))
                sql.And().Where<Customer>(c => c.CompanyName.Contains(name));
            DBRegionData<ExpandoObject> result = new DBRegionData<ExpandoObject>(index, 10);
            result.Execute(db.DBContext, sql);
            return result;
        }
    }
}
