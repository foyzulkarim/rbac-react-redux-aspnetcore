using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuthGrpcService
{
    [Authorize]
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly RedisService redisService;

        public GreeterService(ILogger<GreeterService> logger, RedisService redisService)
        {
            this.redisService = redisService;
            _logger = logger;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var user = context.GetHttpContext().User;
            var userName = user.Identity.Name;
            var claimsIdentity = user.Identities.First() as ClaimsIdentity;
            var claim = claimsIdentity.Claims.First(x => x.Type == "jti");
            var jti = claim.Value;
            string msg = "";
            var inValid = string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(jti) || string.IsNullOrWhiteSpace(request.Name);
            if (inValid)
            {
                return new HelloReply
                {
                    Message = "Unauthorized"
                };
            }

            var redisValue = await redisService.Get(userName);
            if (string.IsNullOrWhiteSpace(redisValue))
            {
                return new HelloReply
                {
                    Message = "Unauthorized"
                };
            }

            var dbValue = (dynamic)JsonConvert.DeserializeObject(redisValue);
            var jtiArray = ((dbValue as dynamic).jtis as dynamic) as JArray;
            var list = jtiArray.ToObject<List<string>>();
            var validJti = list.Exists(x => x == jti);

            if (!validJti)
            {
                return new HelloReply
                {
                    Message = "Unauthorized"
                };
            }

            var permissionViewModels = JsonConvert.DeserializeObject<List<dynamic>>(
                ((dbValue as dynamic).resources as JValue).ToString());
            var permitted = permissionViewModels.Exists(x => x.Name == request.Name && Convert.ToBoolean(x.IsAllowed));

            if (!permitted)
            {
                return new HelloReply
                {
                    Message = "Forbid"
                };
            }

            return new HelloReply
            {
                Message = string.Empty
            };
        }
    }
}
