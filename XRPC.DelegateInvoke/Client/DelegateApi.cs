using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public delegate Task<List<Order>> ListOrders(int employee, string employeeid);

    public delegate Task<List<Employee>> ListEmployees();

    public delegate Task<List<Customer>> ListCustomers();
}
