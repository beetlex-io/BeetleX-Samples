using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.ApiDoc;
using BeetleX.FastHttpApi.Hosting;
using Microsoft.Extensions.Hosting;
using Northwind.Data;
using System;
using System.Linq;

namespace Web.AutoUI
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
                        o.SetDebug();
                        o.LogToConsole = true;
                    },
                    s =>
                    {
                        s.AddExts("woff;tff");
                    },
                    typeof(Program).Assembly, typeof(BeetleX.FastHttpApi.ApiDoc.DocController).Assembly);
                });
            builder.Build().Run();
        }
    }
    [Controller]
    public class Home
    {
        public string Hello(string name)
        {
            return $"hello {name}";
        }


        [Input(Label = "用户名", Name = "name", Eof = true)]
        [Required("用户名不能为空", Name = "name")]
        [Input(Label = "密码", Name = "pwd", Type = "password", Eof = true)]
        [Required("用户密码不能为空", Name = "pwd")]
        [Input(Label = "保存状态", Value = true, Name = "saveStatus")]
        public bool Login(string name, string pwd, bool saveStatus)
        {
            Console.WriteLine($"name:{name} pwd:{pwd} saveStatus:{saveStatus}");
            return name == "admin";
        }

        [Column("FirstName", Type = "link")]
        [Column("LastName", Read = true)]
        [Column("Title")]
        [Column("HomePhone")]
        [Column("City")]
        [Column("Address")]
        public object Employees()
        {
            return DataHelper.Defalut.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return DataHelper.Defalut.Employees.FirstOrDefault(p => p.EmployeeID == id);
        }

        [Post]
        [UIAction(Col = 2, LabelWidth = 150)]
        [Input(Name = "employeeid", Hide = true)]
        [Input(Name = "id", Hide = true)]
        [Input(Name = "Notes", Type = "remark", Row = 5)]
        public Employee SaveEmploye(int id, Employee emp, IHttpContext context)
        {
            Console.WriteLine($"Update employee {id}");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(emp));
            emp.EmployeeID = id;
            return emp;
        }

        [Post]
        public RegisterDto Register(RegisterDto register)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(register));
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



        [UIAction(Col = 0, LabelWidth = 120)]
        [Input(Name = "id", Type = "select", DataUrl = "/EmployeeSelecter", Label = "雇员",NullOption =true)]
        [Input(Name = "customerid", Type = "select", DataUrl = "/CustomerSelecter", Label = "客户",NullOption =true,  Eof = true)]
        [SizeInput(Name = "size", Label = "分页记录数")]
        [Input(Name = "index", Hide = true)]

        [Column("OrderID", Read = true)]
        [Column("EmployeeID", Type = "select", DataUrl = "/EmployeeSelecter")]
        [Column("CustomerID", Type = "select", DataUrl = "/CustomerSelecter")]
        [Column("OrderDate", Type = "date")]
        [Column("RequiredDate", Type = "date")]
        [Column("ShippedDate", Type = "date")]
        public object Orders(int id, string customerid, int index, int size, IHttpContext context)
        {
            Func<Order, bool> exp = o => (id == 0 || o.EmployeeID == id)
             && (string.IsNullOrEmpty(customerid) || o.CustomerID == customerid);
            int count = DataHelper.Defalut.Orders.Count(exp);
            if (size == 0)
                size = 20;
            int pages = count / size;
            if (count % size > 0)
                pages++;
            var items = DataHelper.Defalut.Orders.Where(exp).Skip(index * size).Take(size);
            return new { pages, index, items };

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
        [Input(Label = "用户名", Eof = true)]
        [Required("用户名不能为空")]
        [DataRange("用户名的必须大于3个字符", Min = 3)]
        public string Name { get; set; }

        [Input(Label = "邮箱地址", Eof = true)]
        [Required("邮件地址无效", Type = "email")]
        public string Email { get; set; }

        [Input(Label = "密码", Eof = true, Type = "password")]
        [Required("输入密码")]
        public string Password { get; set; }

        [Input(Label = "确认密码", Eof = true, Type = "password")]
        [Required("输入确认密码")]
        public string ConfirmPassword { get; set; }

        [GenderInput(Label = "性别", Value = "男", Eof = true)]
        public string Gender { get; set; }

        [Required("选择所在城市")]
        [CityInput(Label = "城市", Eof = true)]
        public string City { get; set; }

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
