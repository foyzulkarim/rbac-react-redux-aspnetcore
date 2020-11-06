using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;

namespace WebApplication2.Attributes
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString()
                .Split("Bearer", StringSplitOptions.RemoveEmptyEntries).First();
            var claims = (context.HttpContext.User.Identity as System.Security.Claims.ClaimsIdentity).Claims.ToList();
            var claim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
            var jti = claim.Value;
            string authServerUrl = Constants.AuthServer;
            string url = $"{authServerUrl}/api/AuthorizeToken?jti={jti}";
            HttpClient client = new HttpClient();
            try
            {
                var httpResponseMessage = client.GetAsync(url).GetAwaiter().GetResult();
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}