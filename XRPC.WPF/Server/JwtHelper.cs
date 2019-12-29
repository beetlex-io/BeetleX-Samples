using BeetleX.EventArgs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Server
{
    public class JWTHelper
    {
        public const string TOKEN_KEY = "Token";

        private string mIssuer = null;

        private string mAudience = null;

        private SecurityKey mSecurityKey;

        private SigningCredentials mSigningCredentials;

        private TokenValidationParameters mTokenValidation = new TokenValidationParameters();

        private JwtSecurityTokenHandler mJwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public JWTHelper(string issuer, string audience, byte[] key)
        {
            mIssuer = issuer;
            mAudience = audience;
            mSecurityKey = new SymmetricSecurityKey(key);
            if (string.IsNullOrEmpty(mIssuer))
            {
                mTokenValidation.ValidateIssuer = false;
            }
            else
            {
                mTokenValidation.ValidIssuer = mIssuer;
            }
            if (string.IsNullOrEmpty(mAudience))
            {
                mTokenValidation.ValidateAudience = false;
            }
            else
            {
                mTokenValidation.ValidAudience = mAudience;
            }
            mTokenValidation.IssuerSigningKey = mSecurityKey;
            mSigningCredentials = new SigningCredentials(mSecurityKey, SecurityAlgorithms.HmacSha256);
            Expires = 60 * 24;
        }

        public int Expires { get; set; }

        public string CreateToken(string name, string role, int timeout = 60)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Name", name));
            claimsIdentity.AddClaim(new Claim("Role", role));
            var item = mJwtSecurityTokenHandler.CreateEncodedJwt(mIssuer, mAudience, claimsIdentity, DateTime.Now.AddMinutes(-5),
                DateTime.Now.AddMinutes(timeout), DateTime.Now,
               mSigningCredentials);
            return item;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            return mJwtSecurityTokenHandler.ValidateToken(token, mTokenValidation, out var securityToken);
        }

        public UserInfo GetUserInfo(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    UserInfo userInfo = new UserInfo();
                    var info = ValidateToken(token);
                    ClaimsIdentity identity = info?.Identity as ClaimsIdentity;
                    userInfo.Name = identity?.Claims?.FirstOrDefault(c => c.Type == "Name")?.Value;
                    userInfo.Role = identity?.Claims?.FirstOrDefault(c => c.Type == "Role")?.Value;
                    return userInfo;
                }
            }
            catch {  
            }
            return null;
        }

        public class UserInfo
        {
            public string Name;

            public string Role;
        }

        public static JWTHelper Default
        {
            get;
            set;
        }

        public static void Init()
        {
            byte[] key = new byte[128];
            new Random().NextBytes(key);
            Default = new JWTHelper("xrpc", "xrpc", key);
        }
    }
}
