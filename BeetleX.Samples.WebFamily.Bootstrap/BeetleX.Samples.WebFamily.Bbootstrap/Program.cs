using BeetleX.WebFamily;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetleX.Samples.WebFamily.Bbootstrap
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Title = "beetlex & bootstrap!";
            WebHost.LogoImg = "/images/icons-hero.png";
            WebHost.HeaderModel = "custom-header";
            WebHost.MustLogin = true;
            WebHost.GetMenus = (usr, role, http) =>
            {
                List<Menu> result = new List<Menu>();
                var item = new Menu();
                item.ID = "home";
                item.Img = "fas fa-home";
                item.Name = "主页";
                item.Model = "webfamily-home";

                result.Add(item);


                item = new Menu();
                item.ID = "line";
                item.Img = "fas fa-chart-line";
                item.Name = "折线图";
                var sub = new Menu();
                sub.ID = "line-simple";
                sub.Name = "基础线图";
                sub.Model = "line-simple";
                item.Childs.Add(sub);

                sub = new Menu();
                sub.ID = "line-stack";
                sub.Name = "折线图堆叠";
                sub.Model = "line-stack";
                item.Childs.Add(sub);
                result.Add(item);


                item = new Menu();
                item.ID = "pie";
                item.Img = "fas fa-chart-pie";
                item.Name = "饼图";
                sub = new Menu();
                sub.ID = "pie-simple";
                sub.Name = "基础饼图";
                sub.Model = "pie-simple";
                item.Childs.Add(sub);

                sub = new Menu();
                sub.ID = "pie-roseType";
                sub.Name = "南丁格尔玫瑰图";
                sub.Model = "pie-roseType";
                item.Childs.Add(sub);
                result.Add(item);


                item = new Menu();
                item.ID = "bar";
                item.Img = "fas fa-chart-bar";
                item.Name = "柱状图";
                sub = new Menu();
                sub.ID = "bar-simple";
                sub.Name = "基础柱状图";
                sub.Model = "bar-simple";
                item.Childs.Add(sub);

                sub = new Menu();
                sub.ID = "bar-label-rotation";
                sub.Name = "柱状图标签旋转";
                sub.Model = "bar-label-rotation";
                item.Childs.Add(sub);
                result.Add(item);


                return Task.FromResult(result);

            };
            WebHost host = new WebHost();
            host.RegisterController<Program>()
            .Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Info;
                o.LogToConsole = true;
            })
            .UseJWT()
            .UseFontawesome()
            .UseBootstrap(PageStyle.Bootstrap)
            .Initialize((http, vue, resoure) =>
            {
                resoure.AddCss("website.css");
                resoure.AddScript("echarts.js");
                vue.Debug();
            }).Run();
        }
    }
}
