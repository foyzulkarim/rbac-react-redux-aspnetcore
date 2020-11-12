using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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
            var authServerUrl = Constants.AuthServer;
            var resource = context.HttpContext.Request.Path.Value;
            var url = $"{authServerUrl}/api/AuthorizeToken?resource={resource}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(context.HttpContext.Request.Headers["Authorization"].ToString());
            
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