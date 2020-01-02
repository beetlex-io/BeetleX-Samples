using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public interface IDataService
    {
        Task<string> Login(string name, string pwd);

        Task<List<Employee>> List();
    }
}
