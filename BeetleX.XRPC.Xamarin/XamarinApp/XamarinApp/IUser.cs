using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinApp
{
    public interface IUser
    {
        Task<string> Login(string name, string pwd);
    }
}
