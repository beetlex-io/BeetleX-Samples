using BeetleX.FastHttpApi.Jwt;
using NorthwindEFCoreSqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetleX.WebFamily.Base
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            WebHost.GetMenus = (user, role, context) =>
            {
                List<Menu> menus = new List<Menu>();

                var item = new Menu();
                item.ID = "home";
                item.Img = "fas fa-home";
                item.Name = "主页";
                item.Model = "home";
                menus.Add(item);

                item = new Menu();
                item.ID = "customer";
                item.Name = "客户";
                item.Img = "fas fa-hospital-user";
                item.Model = "customers";
                menus.Add(item);
                return Task.FromResult(menus);
            };
            host.RegisterComponent<Program>()
            .Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Error;
                o.LogToConsole = true;
            })
            .UseEFCore<NorthwindContext>()
            .UseFontawesome()
            .UseElement(PageStyle.ElementDashboard)
            .UseJWT()
            .Initialize((http, vue, rec) =>
            {
                WebHost.LoginHandler = (user, pwd, context) =>
                    {
                        var result = context.SetJwtToken(user, "user", 60 * 60);
                        return Task.FromResult((object)result);
                    };
                vue.Debug();
                WebHost.Title = "Beetelx web SPA plug-in";
            }).Run();
        }
    }
}
