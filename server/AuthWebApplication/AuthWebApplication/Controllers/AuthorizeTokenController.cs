using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthWebApplication.Models.ViewModels;
using AuthWebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuthWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeTokenController : ControllerBase
    {
        private RedisService redisService;

        public AuthorizeTokenController(RedisService redisService)
        {
            this.redisService = redisService;
        }

        public async Task<IActionResult> Get(string userName, string jti, string resource)
        {
            var inValid = string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(jti) || string.IsNullOrWhiteSpace(resource);
            if (inValid)
            {
                return Unauthorized("Invalid data");
            }

            var redisValue = await redisService.Get(userName);
            if (string.IsNullOrWhiteSpace(redisValue))
            {
                return Unauthorized(userName);
            }

            var dbValue = (dynamic)JsonConvert.DeserializeObject(redisValue);
            var jtiArray = ((dbValue as dynamic).jtis as dynamic) as JArray;
            var list = jtiArray.ToObject<List<string>>();
            var validJti = list.Exists(x => x == jti);

            if (!validJti)
            {
                return Unauthorized(jti);
            }

            var permissionViewModels = JsonConvert.DeserializeObject<List<ApplicationPermissionViewModel>>(
                ((dbValue as dynamic).resources as JValue).ToString());
            var permitted = permissionViewModels.Exists(x => x.Name == resource && Convert.ToBoolean(x.IsAllowed));

            if (!permitted)
            {
                return Forbid("Bearer");
            }

            return Ok();
        }
    }
}
