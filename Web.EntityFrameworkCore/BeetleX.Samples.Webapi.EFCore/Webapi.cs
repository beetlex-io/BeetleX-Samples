using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace BeetleX.Samples.Webapi.EFCore
{
    [Controller]
    public class Webapi
    {
        public DBObjectList<Customer> Customers(string name, string country, EFCoreDB<NorthwindContext> db)
        {
            Select<Customer> select = new Select<Customer>();
            if (!string.IsNullOrEmpty(name))
                select &= c => c.CompanyName.StartsWith(name);
            if (!string.IsNullOrEmpty(country))
                select &= c => c.Country == country;
            select.OrderBy(c => c.CompanyName.ASC());
            return (db.DBContext, select);
        }

        [Transaction]
        public void DeleteCustomer(string customer, EFCoreDB<NorthwindContext> db)
        {
            db.DBContext.Orders.Where(o => o.CustomerID == customer).Delete();
            db.DBContext.Customers.Where(c => c.CustomerID == customer).Delete();
        }

        public DBValueList<string> CustomerCountry(EFCoreDB<NorthwindContext> db)
        {
            SQL sql = "select distinct country from customers";
            return (db.DBContext, sql);
        }
    }
}
