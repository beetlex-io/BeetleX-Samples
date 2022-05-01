using BeetleX.FastHttpApi.Hosting;
using BeetleX.FastHttpApi.EFCore.Extension;
using BeetleX.FastHttpApi.VueExtend;
using NorthwindEFCoreSqlite;
using System;

namespace BeetleX.Samples.Webapi.Vuebase
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer(80);
            server.Setting((service, option) =>
            {
                option.SetDebug();
                option.LogLevel = EventArgs.LogType.Warring;
                option.LogToConsole = true;
            }).Creating(http =>
            {
                http.AddEFCoreDB<NorthwindContext>();

            }).Completed(http =>
            {
                //定义Vue资源加载路径
                http.AddExts("woff;ttf");
                http.Vue().JsRewrite("/js/{group}.js").CssRewrite("/css/{group}.css").Debug();
                //定义vuebase的Vue资源项
                var websource = http.Vue().CreateWebResorce("vuebase");
                websource.AddAssemblies(typeof(Program).Assembly);
                //加载基础css
                websource.AddCss("index.css", "site.css");
                //加载基础javascript
                websource.AddScript("axios.js", "vue.js", "element.js", "beetlex4axios.js");
            }).RegisterComponent<Program>();
            server.Run();

        }
    }
}
