using BeetleX.FastHttpApi.Jwt;
using BeetleX.FastHttpApi.VueExtend;
using BeetleX.WebFamily;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetleX.Samples.WebFamily.AdminTheme
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            host.RegisterController<Program>()
            .Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Error;
                o.LogToConsole = true;
            })
            .UseEFCore<NorthwindContext>()
            .UseJWT()
            .Initialize(s =>
            {
                s.GetWebFamily().AddScript("echarts.js");
                s.GetWebFamily().AddCss("website.css");
                s.Vue().Debug();
WebHost.LoginHandler = (user, pwd, context) =>
{
    context.SetJwtToken(user, "user", 60 * 60);
    return Task.CompletedTask;
};
                WebHost.Title = "Northwind";
                WebHost.HeaderModel = "myheader";
                WebHost.MustLogin = true;
                WebHost.HomeModel = "home";
                
                WebHost.GetMenus = (user, role, context) =>
        {
            List<Menu> menus = new List<Menu>();
            var item = new Menu();
            item.ID = "home";
            item.Name = "主页";
            item.Img = "/images/home.png";
            item.Model = "home";
            menus.Add(item);

            item = new Menu();
            item.ID = "product";
            item.Name = "产品";
            item.Img = "/images/product.png";
            item.Model = "products";
            menus.Add(item);

            item = new Menu();
            item.ID = "order";
            item.Name = "订单";
            item.Img = "/images/order.png";
            item.Model = "orders";
            menus.Add(item);

            item = new Menu();
            item.ID = "customer";
            item.Name = "客户";
            item.Img = "/images/customer.png";
            item.Model = "customers";
            menus.Add(item);

            item = new Menu();
            item.ID = "employee";
            item.Name = "雇员";
            item.Img = "/images/employee.png";
            item.Model = "employees";
            menus.Add(item);

            return Task.FromResult(menus);
        };

            }).Run();
        }
    }
}
