using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using BeetleX.WebFamily;
using System;
using System.IO;

namespace Web.BigfileUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost host = new WebHost();
            host.RegisterComponent<Program>()
            .Setting(o =>
            {
                o.SetDebug();
                o.Port = 80;
                o.LogLevel = LogType.Warring;
                o.LogToConsole = true;
            })
            .Initialize((http, vue, rec) =>
            {
                rec.AddCss("website.css");
                vue.Debug();
            }).Run();
        }
    }

    [Controller]
    class Webapi
    {
        private System.Collections.Concurrent.ConcurrentDictionary<string, System.IO.Stream> mStreams
            = new System.Collections.Concurrent.ConcurrentDictionary<string, System.IO.Stream>(StringComparer.OrdinalIgnoreCase);
        [Put]
        [NoDataConvert]
        public void Upload(string name, bool first, bool eof, IHttpContext context)
        {
            Console.WriteLine($"{name}-{first}-{eof}-{context.Request.Stream.Length}");
            Stream stream;
            if (first)
            {
                stream = System.IO.File.Create(name);
                mStreams[name] = stream;
            }
            else
            {
                mStreams.TryGetValue(name, out stream);
            }
            context.Request.Stream.CopyTo(stream);
            if (eof)
            {
                stream.Flush();
                stream.Dispose();
                mStreams.TryRemove(name, out stream);
            }
        }
    }
}
