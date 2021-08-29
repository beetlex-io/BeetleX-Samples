using BeetleX.Http.Clients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Samples.Webapi.GithubOAuth
{
    public class GithubAuth
    {
        static GithubAuth()
        {
            githubAuth = HttpApiClient.Create<IGithubAuth>(20000);
        }

        private static IGithubAuth githubAuth;

        public string ClientID { get; set; }

        public string ClientSecret { get; set; }

        private Dictionary<string, string> GetNameValues(string value)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in value.Split('&'))
            {
                var values = item.Split('=');
                if (values.Length > 1)
                {
                    result[values[0]] = values[1];
                }
            }
            return result;
        }

        public async Task<string> GetToken(string code)
        {
            var data = await githubAuth.GetToken(ClientID, ClientSecret, code);
            var result = GetNameValues(data);
            return result["access_token"];
        }

        public async Task<GithubUserInfo> GetUserInfo(string token)
        {
            var data = await githubAuth.GetUser(token);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<GithubUserInfo>(data);
        }
    }

    [FormUrlFormater]
    [Host("https://github.com")]
    public interface IGithubAuth
    {

        [Get(Route = "login/oauth/access_token")]
        Task<string> GetToken(string client_id, string client_secret, string code);

        [Host("https://api.github.com")]
        [Header("User-Agent", "beetlex.io")]
        [Get(Route = "user")]
        Task<string> GetUser(string access_token);
    }

    public class GithubUserInfo
    {
        public string login { get; set; }

        public string id { get; set; }

        public string avatar_url { get; set; }

        public string email { get; set; }

    }
}
