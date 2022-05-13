using BeetleX.ESDoc;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Hosting;
using BeetleX.FastHttpApi.VueExtend;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace BeetleX.ESDocApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer(80);
            server.Setting((service, option) =>
            {
                option.SetDebug();
                option.LogToConsole = true;
            })
            .RegisterComponent<Program>()
            .Completed(http =>
            {

                http.AddExts("ttf;woff");
                var vue = http.Vue();
                vue.Debug();
                var resource = vue.CreateWebResorce("beetlex");
                vue.JsRewrite("/js/{group}-{v}.js").CssRewrite("/css/{group}-{v}.css");
                resource.AddCss("element.css", "website.css")
                .AddScript("vue.js", "axios.js", "beetlex4axios.js", "element.js", "website.js");
var document = new DocumentDB("document_test");
document.Init("http://localhost:9200").Wait(10000);
                http["es-db"] = document;
                ImportData(document);
            });
            server.Run();
        }
        static void ImportData(DocumentDB doc)
        {
            BeetleX.EFCore.Extension.XDBContext.SetDefaultDB<BlogDB>();
            BeetleX.EFCore.Extension.SQL2ObjectList<Document> items = "select Name as Category,Posts.ID,Tag,Content,Summary,Title,CreateTime from Posts inner join Projects on Posts.Project=Projects.ID";
            foreach (var item in items)
            {
                doc.Put(item);
            }


        }
    }

    public partial class BlogDB : DbContext
    {
        public BlogDB()
        {
        }

        public BlogDB(DbContextOptions<BlogDB> options)
            : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=Blog.db");
            }
        }
    }
    [ParameterObjectMapper(typeof(DocumentDB))]
    public class DocumentBinder : BeetleX.FastHttpApi.ParameterBinder
    {
        public override bool DataParameter => false;
        public override object GetValue(IHttpContext context)
        {
            return context.Server["es-db"];
        }
    }
}
