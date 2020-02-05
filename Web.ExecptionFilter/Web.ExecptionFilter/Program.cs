using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Web.ExecptionFilter
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
                        o.LogToConsole = true;
                        o.ManageApiEnabled = false;
                        o.Port = 80;
                        o.SetDebug();
                        o.Filters.Add(new ExceptionFilter());
                        o.LogLevel = BeetleX.EventArgs.LogType.Warring;
                    },
                    typeof(Program).Assembly);
                });
            builder.Build().Run();
        }
    }
    [Controller]
    public class Home
    {
       
        public object Hello()
        {
            throw new Exception("hello error!");
        }
      
    }

    public class ExceptionFilter : FilterAttribute {
        public override void Executed(ActionContext context)
        {
            if(context.Exception !=null)
            {
                context.Result = new TextResult($"catch error {context.Exception.Message}");
                context.Exception = null;
            }
            base.Executed(context);
        }
    }


}
