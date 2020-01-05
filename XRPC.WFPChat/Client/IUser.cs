using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public interface IUser
    {
        Task Login(string name);

        Task Talk(string name, string message);

        Task Exit(string name);

    }
}
