using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthWebApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Get(string jti)
        {
            var s = await redisService.Get(jti);
            var inValid = string.IsNullOrWhiteSpace(s);
            return inValid ? (IActionResult) Unauthorized(jti) : Ok();
        }
    }
}
