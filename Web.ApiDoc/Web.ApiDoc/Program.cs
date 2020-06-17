using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using Microsoft.Extensions.Hosting;
using BeetleX.FastHttpApi.ApiDoc;
using System;
using Northwind.Data;
using System.Linq;
namespace Web.ApiDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.UseBeetlexHttp(o =>
                    {
                        o.Port = 80;
                        o.AutoGzip = true;
                        o.LogLevel = BeetleX.EventArgs.LogType.Info;
                        o.LogToConsole = true;
                    },
                   s =>
                   {
                       s.UseApiDoc();
                   }, typeof(DocTestHome).Assembly);

                });
            builder.Build().Run();
            //http://localhost/beetlex/apidoc/
        }
    }
    [Controller]
    public class DocTestHome
    {
        public string Hello(string name)
        {
            return $"hello {name}";
        }

        public bool Login(
            [Input(Label = "用户名")]
             [Required("用户名不能为空")]
            string name,
            [Input(Label = "密码")]
             [Required("用户密码不能为空")]
            string pwd,
            [Input(Label = "保存状态", Value = true)]
            bool saveStatus)
        {
            return name == "admin";
        }

        public Employee GetEmployee(int id)
        {
            return DataHelper.Defalut.Employees.FirstOrDefault(p => p.EmployeeID == id);
        }
        [Post]
        [UIAction(Col = 3, LabelWidth = 150)]
        public Employee AddEmploye(Employee emp)
        {
            return emp;
        }

        [Post]
        public RegisterDto Register(RegisterDto register)
        {
            return register;
        }

        public object EmployeeSelecter()
        {
            return from e in DataHelper.Defalut.Employees select new { value = e.EmployeeID, label = e.FirstName + " " + e.LastName };
        }

        public object CustomerSelecter()
        {
            return from c in DataHelper.Defalut.Customers select new { value = c.CustomerID, label = c.CompanyName };
        }
        [UIAction(Col = 0, LabelWidth = 80)]
        public object Orders(
            [Input(Type = "select", DataUrl = "/EmployeeSelecter", Label = "雇员")]
            int id,
            [Input(Type = "select", DataUrl = "/CustomerSelecter", Label = "客户")]
            string customerid,
            [Input(Label ="页数")]
            int index,
            [SizeInput(Label ="显示数量")]
            int size, IHttpContext context)
        {
            Func<Order, bool> exp = o => (id == 0 || o.EmployeeID == id)
             && (string.IsNullOrEmpty(customerid) || o.CustomerID == customerid);
            int count = DataHelper.Defalut.Orders.Count(exp);
            if (size == 0)
                size = 10;
            int pages = count / size;
            if (count % size > 0)
                pages++;
            var items = DataHelper.Defalut.Orders.Where(exp).Skip(index * size).Take(size);
            return items;

        }

    }

    public class SizeInput : InputAttribute
    {
        public SizeInput()
        {
            Type = "select";
            Value = 20;
        }
        public override object Data => new object[] { new { value = 10, label = 10 }, new { value = 20, label = 20 }, new { value = 50, label = 50 } };
    }

    public class RegisterDto
    {
        [Input(Label = "用户名")]
        [Required("用户名不能为空")]
        [DataRange("用户名的必须大于3个字符", Min = 3)]
        public string Name { get; set; }
        [Input(Label = "邮箱地址")]
        [Required("邮件地址不能为空", Type = "email")]
        public string Email { get; set; }
        [GenderInput(Label = "性别", Value = "男")]
        public string Gender { get; set; }
        [Required("选择所在城市")]
        [CityInput(Label = "城市")]
        public string City { get; set; }
        [Input(Label = "密码")]
        public string Password { get; set; }
        [HobbyInput(Label = "爱好")]
        [Required("选择爱好", Type = "array", Trigger = "change")]
        public string[] Hobby { get; set; }
    }


    public class GenderInput : InputAttribute
    {
        public GenderInput()
        {
            Type = "radio";
        }

        public override object Data => new object[] { new { value = "男", label = "男" }, new { value = "女", label = "女" } };
    }

    public class CityInput : InputAttribute
    {
        public CityInput()
        {
            Type = "select";
        }

        public override object Data => new object[] {
            new { value = "广州", label = "广州" },
            new { value = "深圳", label = "深圳" },
            new { value = "上海", label = "上海" },
            new { value = "北京", label = "北京" }};
    }

    public class HobbyInput : InputAttribute
    {
        public HobbyInput()
        {
            Type = "checkbox";
        }
        public override object Data => new object[] {
            new { value = "打球", label = "打球" },
            new { value = "游泳", label = "游泳" },
            new { value = "爬山", label = "爬山" },
            new { value = "游戏", label = "游戏" }};
    }



}
