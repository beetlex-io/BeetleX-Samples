using BeetleX.WebFamily;
using System;
using System.Threading.Tasks;
using System.Linq;
using BeetleX.WebFamily.Searches;
using BeetleX.FastHttpApi.EFCore.Extension;
using BeetleX.FastHttpApi;

namespace BeetleX.Samples.WebFamily.ElasticSearch
{
    [BeetleX.FastHttpApi.Controller]
    public class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            host.RegisterComponent<Program>();
            host.UserElasticSearch("test_query_string", "http://192.168.2.19:9200");
            host.UseEFCore<BlogDBContext>();
            host.Setting(o =>
            {
                o.Port = 80;
                o.LogLevel = EventArgs.LogType.Info;
                o.LogToConsole = true;
            })
            .Run();
        }

        public async Task<object> Search(IHttpContext context)
        {
            var search = context.Server.GetDocument().CreateSearch(0, 20);
            search.QueryText = "beetlex websocket";
            search.Highlight = true;
            var items = await search.Execute();
            return items;
        }

        public async Task Import(IHttpContext context, EFCoreDB<BlogDBContext> db)
        {
            var doc = context.Server.GetDocument();
            var items = from a in db.DBContext.Posts select a;
            foreach (var item in items)
                await doc.Put(new Document { Category = item.Project, Content = item.Content, CreateTime = item.CreateTime, ID = item.ID, Tag = item.Tag, Title = item.Title });

        }
    }
}
