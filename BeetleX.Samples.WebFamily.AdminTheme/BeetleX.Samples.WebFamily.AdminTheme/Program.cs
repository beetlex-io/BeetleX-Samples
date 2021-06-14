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
            .UseFontawesome()
            .UseElement(PageStyle.ElementDashboard)

            .Initialize((http, vue, rec) =>
            {
                rec.AddScript("echarts.js");
                rec.AddCss("website.css");
                vue.Debug();
                WebHost.LoginHandler = (user, pwd, context) =>
                {
                   var result=  context.SetJwtToken(user, "user", 60 * 60);
                    return Task.FromResult((object)result);
                };
                WebHost.Title = "Northwind";
                WebHost.HeaderModel = "myheader";
                WebHost.Login = true;
                WebHost.HomeModel = "home";

                WebHost.GetMenus = (user, role, context) =>
        {
            List<Menu> menus = new List<Menu>();
            var item = new Menu();
            item.ID = "home";
            item.Name = "主页";
            item.Img = "fas fa-home";
            item.Model = "home";
            menus.Add(item);

            item = new Menu();
            item.ID = "product";
            item.Name = "产品";
            item.Img = "fas fa-box";
            item.Model = "products";
            menus.Add(item);

            item = new Menu();
            item.ID = "order";
            item.Name = "订单";
            item.Img = "fas fa-shopping-cart";
            item.Model = "orders";
            menus.Add(item);

            item = new Menu();
            item.ID = "customer";
            item.Name = "客户";
            item.Img = "fas fa-hospital-user";
            item.Model = "customers";
            menus.Add(item);

            item = new Menu();
            item.ID = "employee";
            item.Name = "雇员";
            item.Img = "fas fa-users";
            item.Model = "employees";
            menus.Add(item);

            return Task.FromResult(menus);
        };

            }).Run();
        }
    }
}
