using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using AuthWebApplication.Services;
using AuthWebApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthWebApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly SecurityDbContext securityDb;
        private readonly ILogger<TokenController> logger;
        private readonly RedisService redisService;

        public LogoutController(ILogger<TokenController> logger, UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions, SecurityDbContext securityDb, RedisService redisService)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions.Value;
            this.securityDb = securityDb;
            this.redisService = redisService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LogoutViewModel logoutViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.Identity.Name;
            ApplicationUserToken? token = this.securityDb.ApplicationUserTokens.FirstOrDefault(x => x.Name == username && x.Jti == logoutViewModel.Jti);

            if (token != null)
            {
                this.securityDb.ApplicationUserTokens.Remove(token);
                await this.securityDb.SaveChangesAsync();

                var s = await redisService.Get(token.Name);

                await redisService.RemoveJti(username, logoutViewModel.Jti);
            }

            

            return Ok();
        }

    }
}
