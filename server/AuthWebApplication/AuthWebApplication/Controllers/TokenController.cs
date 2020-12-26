using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthLibrary.Services;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using AuthWebApplication.Models.ViewModels;
using AuthWebApplication.Services;
using AuthWebApplication.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AuthWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly SecurityDbContext securityDb;
        private readonly ILogger<TokenController> logger;
        private readonly RedisService redisService;

        public TokenController(ILogger<TokenController> logger, UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions, SecurityDbContext securityDb, RedisService redisService)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions.Value;
            this.securityDb = securityDb;
            this.redisService = redisService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClaimsIdentity identity = await GetClaimsIdentity(loginViewModel.Username, loginViewModel.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.",
                    ModelState));
            }

            Claim claim = identity.Claims.First(x => x.Type == Constants.Strings.JwtClaimIdentifiers.Id);
            var userId = claim.Value.ToString();
            ApplicationUser user = securityDb.Users.First(x => x.Id == userId);

            if (user == null)
            {
                logger.LogError("Invalid login attempt for {UserName}", loginViewModel.Username);
                return BadRequest("The user name or password is incorrect.");
            }

            if (user.IsActive == false)
            {
                logger.LogError("Invalid login attempt for {UserName}", user.UserName);
                return BadRequest("User is deactivated");
            }

            var userRoles = securityDb.ApplicationUserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();
            var roles = securityDb.ApplicationRoles.Where(x => userRoles.Contains(x.Id)).Select(x => x.Id).ToList();

            //List<ApplicationPermissionViewModel> resources = new List<ApplicationPermissionViewModel>();
            List<dynamic> resources = new List<dynamic>();
            if (roles != null && roles.Count > 0)
            {
                resources = securityDb.Permissions.Include(x => x.Resource).Include(x => x.Role).Where(x => roles.Contains(x.RoleId) && x.IsAllowed).Select(x => x.GetMinimalViewModel())
                    .ToList();
            }

            dynamic jwt = await Tokens.GenerateJwt(
                identity,
                jwtFactory,
                jwtOptions,
                user,
                roles,
                resources);

            var jtiClaim = identity.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti);

            var token = new ApplicationUserToken()
            {
                UserId = user.Id,
                Name = user.UserName,
                LoginProvider = jtiClaim.Value,
                Value = true.ToString(),
                Jti = jtiClaim.Value
            };

            await securityDb.UserTokens.AddAsync(token);
            await securityDb.SaveChangesAsync();
            await redisService.Set(token.Name, jwtOptions.ValidFor, token.Jti, resources);
            return Ok(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = userManager.Users.FirstOrDefault(x => x.UserName == userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                ClaimsIdentity identity =
                    jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id);
                ClaimsIdentity claimsIdentity = await Task.FromResult(identity);

                return claimsIdentity;
            }

            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}