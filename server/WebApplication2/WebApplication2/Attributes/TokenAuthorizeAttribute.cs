using System;
using System.Configuration;
using System.Linq;
using System.Net;
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
            var claims = (context.HttpContext.User.Identity as System.Security.Claims.ClaimsIdentity).Claims.ToList();
            var claim = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
            var jti = claim.Value;
            string authServerUrl = Constants.AuthServer;
            string resource = context.HttpContext.Request.Path.Value;
            var name = context.HttpContext.User.Identity.Name;
            string url = $"{authServerUrl}/api/AuthorizeToken?userName={name}&jti={jti}&resource={resource}";
            HttpClient client = new HttpClient();
            try
            {
                var httpResponseMessage = client.GetAsync(url).GetAwaiter().GetResult();
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpRequestException)
            {
                context.Result = httpRequestException.StatusCode switch
                {
                    HttpStatusCode.Forbidden => new ForbidResult("Bearer"),
                    HttpStatusCode.InternalServerError => new BadRequestResult(),
                    _ => new UnauthorizedResult()
                };
            }
            catch (Exception e)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}