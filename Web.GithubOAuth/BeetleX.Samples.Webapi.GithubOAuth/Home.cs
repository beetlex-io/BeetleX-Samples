using BeetleX.FastHttpApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Samples.Webapi.GithubOAuth
{
    [Controller]
    public class Home
    {
        //github回调
        public async Task<object> Github(string code, IHttpContext context)
        {
            var githubAuth = new GithubAuth
            {
                ClientID = "************",
                ClientSecret = "************"
            };
            var token = await githubAuth.GetToken(code);
            var githubUser = await githubAuth.GetUserInfo(token);
            if (githubUser != null)
            {
                return githubUser;
            }
            return "无法获取Github信息";
        }
    }
}
