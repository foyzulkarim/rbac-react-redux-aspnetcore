using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using Grpc.Core;
using Grpc.Net.Client;
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
            var resource = context.HttpContext.Request.Path.Value;
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();
            try
            {
               HttpAuthorization(context, resource);

               // GrpcAuthorization(token, resource);
               
               //GoAuthorization(context, resource);
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

        private void GoAuthorization(AuthorizationFilterContext context, string resource)
        {
            var client = Factory.HttpClient;
            var user = context.HttpContext.User.Identity.Name;
            var claimsIdentity = context.HttpContext.User.Identities.First() as ClaimsIdentity;
            var claim = claimsIdentity.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti);
            var jti = claim.Value;
            var url = $"http://localhost:8080/api/AuthorizeToken?user={user}&resource={resource}&jti={jti}";
            var httpResponseMessage = client.GetAsync(url).GetAwaiter().GetResult();
            httpResponseMessage.EnsureSuccessStatusCode();
        }

        private string HttpAuthorization(AuthorizationFilterContext context, string resource)
        {
            var client = Factory.HttpClient;
            var user = context.HttpContext.User.Identity.Name;
            var claimsIdentity = context.HttpContext.User.Identities.First() as ClaimsIdentity;
            var claim = claimsIdentity.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti);
            var jti = claim.Value;            
            var url = $"{Constants.AuthServer}/api/AuthorizeToken?user={user}&resource={resource}&jti={jti}";
            var httpResponseMessage = client.GetAsync(url).GetAwaiter().GetResult();
            httpResponseMessage.EnsureSuccessStatusCode();
            return jti;
        }


        private void GrpcAuthorization(string token, string resource)
        {
            Greeter.GreeterClient gClient = Factory.GreeterClient;

            var headers = new Metadata
            {
                { "Authorization", $"{token}" }
            };

            var reply = gClient.SayHelloAsync(new HelloRequest { Name = resource }, headers).GetAwaiter().GetResult();
            var replyMessage = reply.Message;
            if (!string.IsNullOrWhiteSpace(replyMessage))
            {
                throw replyMessage switch
                {
                    "Forbid" => new HttpRequestException(),
                    _ => new UnauthorizedAccessException()
                };
            }
        }

        public static class Factory
        {
            private static Greeter.GreeterClient _greeterClient;

            private static HttpClient _httpClient;

            public static Greeter.GreeterClient GreeterClient
            {
                get
                {
                    if (_greeterClient == null)
                    {
                        GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5005");
                        _greeterClient = new Greeter.GreeterClient(channel);
                    }

                    return _greeterClient;
                }
            }

            public static HttpClient HttpClient
            {
                get
                {
                    if (_httpClient == null)
                    {
                        _httpClient = new HttpClient();
                    }

                    return _httpClient;
                }
            }
        }
    }
}