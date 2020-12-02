using BeetleX.EFCore.Extension;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.EFCore.Extension;
using NorthwindEFCoreSqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace BeetleX.Samples.WebFamily.AdminTheme
{
    [Controller]
    public class Home
    {
        public object OrderStats(EFCoreDB<NorthwindContext> db)
        {
            SQL sql = @"select o.ProductID,d.OrderDate,(o.UnitPrice*o.Quantity) Amount,p.ProductName from  'Order Details' o inner join orders d on o.OrderID=d.OrderID
inner join Products p on p.ProductID = o.ProductID order by o.OrderID asc";
            DBObjectList<ExpandoObject> items = (db.DBContext, sql, new EFCore.Extension.Region(0, 10000));

            var productOrderGroup = (from a in items.Cast<dynamic>()
                                     group a by new { a.ProductID, a.ProductName } into g
                                     select new { g.Key.ProductID, g.Key.ProductName, Amount = g.Sum(i => (double)i.Amount) }).OrderByDescending(t => t.Amount).Take(10);

            var timeGroup = from a in items.Cast<dynamic>()
                            group a by DateTime.Parse(a.OrderDate).ToString("yyyy-MM")
                        into g
                            select new { Date = g.Key, Amount = g.Sum(i => (double)i.Amount) };

            var yearMonth = from a in timeGroup select a.Date;

            Dictionary<string, IEnumerable> result = new Dictionary<string, IEnumerable>();
            result["All"] = timeGroup;
            foreach (var item in productOrderGroup)
            {
                var pgroup = from a in items.Cast<dynamic>()
                             where a.ProductID == item.ProductID
                             group a by DateTime.Parse(a.OrderDate).ToString("yyyy-MM")
                        into g
                             select new { Date = g.Key, Amount = g.Sum(i => (double)i.Amount) };
                result[item.ProductName] = pgroup;
            }
            List<object> stats = new List<object>();
            List<string> products = new List<string>();
            foreach (var item in result)
            {
                var statItem = new List<double>();
                products.Add(item.Key);
                foreach (var month in yearMonth)
                {
                    var data = item.Value.Cast<dynamic>().FirstOrDefault(p => p.Date == month);
                    if (data != null)
                        statItem.Add(data.Amount);
                    else
                        statItem.Add(0);
                }
                stats.Add(statItem);
            }
            return new { Months = yearMonth, Products = products, Items = stats };
        }

        public DBObjectList<ExpandoObject> EmployeeStats(EFCoreDB<NorthwindContext> db)
        {
            SQL sql = @"select (e.FirstName || ' '|| e.LastName) name, sum((o.UnitPrice * o.Quantity)) value from  'Order Details' o inner join orders d on o.OrderID = d.OrderID
                    inner join Employees e on e.EmployeeID = d.EmployeeID
                    group by e.FirstName,e.LastName";
            return (db.DBContext, sql);
        }

        public DBObjectList<ExpandoObject> CustomerStats(EFCoreDB<NorthwindContext> db)
        {
            SQL sql = @"select c.CompanyName name, sum((o.UnitPrice*o.Quantity)) value from  'Order Details' o inner join orders d on o.OrderID=d.OrderID
                    inner join Customers c on c.CustomerID= d.CustomerID
                    group by c.CompanyName order by value desc";
            return (db.DBContext, sql, new EFCore.Extension.Region(0, 10));
        }

        public DBObjectList<ExpandoObject> ProductStats(EFCoreDB<NorthwindContext> db)
        {
            SQL sql = @"select p.ProductName name, sum((o.UnitPrice*o.Quantity)) value from  'Order Details' o 
                    inner join Products p on o.ProductID= p.ProductID 
                    group by name order by value desc";
            return (db.DBContext, sql, new EFCore.Extension.Region(0, 10));
        }

        public object Categories(EFCoreDB<NorthwindContext> db)
        {
            return from a in db.DBContext.Categories
                   select new
                   {
                       Count = db.DBContext.Products.Where(p => p.CategoryID == a.CategoryId).Count(),
                       a.CategoryId,
                       a.CategoryName,
                       Description = Encoding.UTF8.GetString(a.Description)
                   };
        }

        public object CategoriesSelectOptions(EFCoreDB<NorthwindContext> db)
        {
            return from a in db.DBContext.Categories select new { label = a.CategoryName, value = a.CategoryId };
        }

        public DBObjectList<ExpandoObject> EmployeesSelectOptions(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select EmployeeID value, (FirstName || ' ' || LastName) label from employees", new EFCore.Extension.Region(0, 1000));
        }

        public DBObjectList<ExpandoObject> CustomersSelectOptions(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select CustomerID value,CompanyName label from customers", new EFCore.Extension.Region(0, 1000));
        }


        public DBObjectList<ExpandoObject> CustomerCountrySelectOptions(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select DISTINCT country value from customers");
        }

        public DBObjectList<ExpandoObject> Employees(EFCoreDB<NorthwindContext> db)
        {
            return (db.DBContext, "select *,(FirstName || ' '|| LastName) Name from employees", new EFCore.Extension.Region(0, 10000));
        }

        public object Customers(EFCoreDB<NorthwindContext> db, string country, string name, int index)
        {
            SQL sql = "select * from customers where 1=1";
            if (!string.IsNullOrEmpty(country))
                sql.And().Where<Customer>(c => c.Country == country);
            if (!string.IsNullOrEmpty(name))
                sql.And().Where<Customer>(c => c.CompanyName.Contains(name));
            DBRegionData<ExpandoObject> result = new DBRegionData<ExpandoObject>(index, 20);
            result.Execute(db.DBContext, sql);
            return result;
        }

        public object Products(EFCoreDB<NorthwindContext> db, int index, string name, int category)
        {
            SQL sql = @"select products.*,categories.CategoryName from products  inner join categories
                        on products.CategoryID = categories.CategoryID where 1=1";
            if (!string.IsNullOrEmpty(name))
                sql.And().Where<Product>(p => p.ProductName.StartsWith(name));
            if (category > 0)
                sql.And().Where<Product>(p => p.CategoryID == category);
            DBRegionData<ExpandoObject> result = new DBRegionData<ExpandoObject>(index, 20);
            result.Execute(db.DBContext, sql);
            return result;
        }

        public object Orders(EFCoreDB<NorthwindContext> db, int index, int product, int employee, string customer, string jwt_user, string jwt_role)
        {
            Console.WriteLine($"jwt_info:{jwt_user}/{jwt_role}");
            SQL sql = @"select orders.*,(employees.FirstName || ' ' || employees.LastName) Employee,
                        customers.CompanyName Customer from orders inner join employees
                        on orders.EmployeeID = employees.EmployeeID inner
                               join customers
                        on orders.CustomerID = customers.CustomerID where 1=1";
            if (employee > 0)
                sql.And().Where<Employee>(e => e.EmployeeID == employee);
            if (!string.IsNullOrEmpty(customer))
                sql.And().Where<Customer>(c => c.CustomerID == customer);
            if (product > 0)
            {
                sql += " and orders.OrderID in(select orderid from 'Order Details' where ProductID=@p1)";
                sql += ("@p1", product);
            }
            DBRegionData<ExpandoObject> result = new DBRegionData<ExpandoObject>(index, 10);
            result.Execute(db.DBContext, sql);
            foreach (dynamic item in result.Items)
            {
                sql = @"select [Order Details].*, Products.ProductName from [Order Details] inner join Products
                    on [Order Details].ProductID= Products.ProductID Where [Order Details].OrderID=" + item.OrderID;
                item.Details = sql.List<ExpandoObject>(db.DBContext);
            }
            return result;
        }
    }
}
