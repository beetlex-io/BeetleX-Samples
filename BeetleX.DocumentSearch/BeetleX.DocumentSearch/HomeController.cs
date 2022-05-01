using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeetleX.ESDoc;
using BeetleX.FastHttpApi;
namespace BeetleX.ESDocApp
{
    [BeetleX.FastHttpApi.Controller]
    public class HomeController
    {
        public void AddDocument(BeetleX.ESDoc.Document body, DocumentDB document)
        {
            if (string.IsNullOrEmpty(body.ID))
            {
                body.ID = Guid.NewGuid().ToString("N");
                body.CreateTime = DateTime.Now;
            }
            document.Put(body);
        }
        public async Task<Document> GetDocument(string id, DocumentDB document)
        {
            return await document.GetDocument(id);
        }
        public async Task<Tuple<IList<Document>, long>> Search(string searchText, int page, DocumentDB document)
        {
            var search = document.CreateSearch(page, 10);
            search.QueryText = searchText;
            return await search.Execute();
        }

        public async Task<object> AggsCategories(DocumentDB document)
        {
            return await document.AggsCategories();
        }

        public async Task<object> AggsTags(DocumentDB document)
        {
            return await document.AggsTag();
        }
        public async Task<object> AggsYear(DocumentDB document)
        {
            return await document.AggsYear();
        }

        public async Task<object> AggsYearMonth(DocumentDB document)
        {
            return await document.AggsMonth();
        }
    }
}
