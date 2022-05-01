using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Samples.Webapi.Vuebase
{
    [Controller]
    public class Webapi
    {
        public SQL2ObjectList<Employee> Employees(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select * from employees");
        }

        public async Task<object> Customers(EFCoreDB<NorthwindContext> db, int size, int index, string country)
        {
            DBRegionData<Customer> result = new DBRegionData<Customer>(index, size);
            Select<Customer> select = new Select<Customer>();
            if (!string.IsNullOrEmpty(country))
                select &= c => c.Country == country;
            await result.ExecuteAsync(db.DBContext, select);
            return result;
        }

        public async Task<object> Orders(EFCoreDB<NorthwindContext> db, int size, int index, string customer, int employee)
        {
            DBRegionData<ExpandoObject> result = new DBRegionData<ExpandoObject>(index, size);
            SQL sql = @"select Orders.*,Employees.FirstName,Employees.LastName,Customers.CompanyName 
from Orders inner join Employees on Employees.EmployeeID=Orders.EmployeeID inner join Customers on Customers.CustomerID= Orders.CustomerID where 1=1";
            if (!string.IsNullOrEmpty(customer))
                sql.And().Where<Order>(o => o.CustomerID == customer);
            if (employee > 0)
                sql.And().Where<Order>(o => o.EmployeeID == employee);
            await result.ExecuteAsync(db.DBContext, sql);
            return result;
        }

        public SQL2ObjectList<string> CustomerCountry(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select distinct country from customers");
        }

        public SQL2ObjectList<ExpandoObject> ListCustomerName(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select CustomerID,CompanyName from customers");
        }

        public SQL2ObjectList<ExpandoObject> ListEmployeeName(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select EmployeeID,FirstName,LastName from employees");
        }
    }
}
