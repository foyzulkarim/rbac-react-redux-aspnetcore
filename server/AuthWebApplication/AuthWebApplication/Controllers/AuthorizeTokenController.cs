using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthLibrary.Services;
using AuthWebApplication.Models.ViewModels;
using AuthWebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuthWebApplication.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeTokenController : ControllerBase
    {
        private RedisService redisService;
        private readonly ILogger<AuthorizeTokenController> logger;

        public AuthorizeTokenController(ILogger<AuthorizeTokenController> logger, RedisService redisService)
        {
            this.logger = logger;
            this.redisService = redisService;
        }

        // public async Task<IActionResult> Get(string resource)
        // {
        //     var userName = this.User.Identity.Name;
        //     var claimsIdentity = this.User.Identities.First() as ClaimsIdentity;
        //     var claim = claimsIdentity.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti);
        //     var jti = claim.Value;

        //     var inValid = string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(jti) || string.IsNullOrWhiteSpace(resource);
        //     if (inValid)
        //     {
        //         return Unauthorized("Invalid data");
        //     }

        //     var redisValue = await redisService.Get(userName);
        //     if (string.IsNullOrWhiteSpace(redisValue))
        //     {
        //         return Unauthorized(userName);
        //     }

        //     var dbValue = (dynamic)JsonConvert.DeserializeObject(redisValue);
        //     var jtiArray = ((dbValue as dynamic).jtis as dynamic) as JArray;
        //     var list = jtiArray.ToObject<List<string>>();
        //     var validJti = list.Exists(x => x == jti);

        //     if (!validJti)
        //     {
        //         return Unauthorized(jti);
        //     }

        //     var permissionViewModels = JsonConvert.DeserializeObject<List<ApplicationPermissionViewModel>>(
        //         ((dbValue as dynamic).resources as JValue).ToString());
        //     var permitted = permissionViewModels.Exists(x => x.Name == resource && Convert.ToBoolean(x.IsAllowed));

        //     if (!permitted)
        //     {
        //         return Forbid("Bearer");
        //     }

        //     return Ok();
        // }

        public async Task<IActionResult> Get(string user, string resource, string jti)
        {
            var inValid = string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(jti) || string.IsNullOrWhiteSpace(resource);
            if (inValid)
            {
                return Unauthorized("Invalid data. User or resource or jti cannot be empty");
            }

            var redisValue = await redisService.Get(user);
            if (string.IsNullOrWhiteSpace(redisValue))
            {
                return Unauthorized(user);
            }

            var dbValue = JsonConvert.DeserializeObject<AuthorizationDataModel>(redisValue);

            if (dbValue == null)
            {
                return Unauthorized("Invalid cache. Please logout and do a fresh login");
            }

            var validJti = dbValue.jtis.Exists(x => x == jti);

            if (!validJti)
            {
                return Unauthorized(jti);
            }

            // var permissionViewModels = JsonConvert.DeserializeObject<List<ApplicationPermissionViewModel>>(
            //     ((dbValue as dynamic).resources as JValue).ToString());
            // var permitted = permissionViewModels.Exists(x => x.Name == resource && Convert.ToBoolean(x.IsAllowed));

            // if (!permitted)
            // {
            //     return Forbid("Bearer");
            // }

            return Ok();
        }

        private class AuthorizationDataModel
        {
            public List<string> jtis { get; set; }

            public List<AuthorizationResourceModel> resources { get; set; }
        }

        private class AuthorizationResourceModel
        {
            public string Name { get; set; }

            public string IsAllowed { get; set; }

            public string IsDisabled { get; set; }
        }
    }
}
