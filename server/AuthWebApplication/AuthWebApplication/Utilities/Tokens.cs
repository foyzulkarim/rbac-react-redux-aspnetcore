using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using AuthWebApplication.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AuthWebApplication.Utilities
{
    public class Tokens
    {
        public static async Task<object> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory,
            JwtIssuerOptions jwtOptions, ApplicationUser user, List<dynamic> roles,
            JsonSerializerSettings serializerSettings, SecurityDbContext db)
        {
            string id = identity.Claims.Single(c => c.Type == "id").Value;
            var name = user.FirstName + " " + user.LastName;
            string token = await jwtFactory.GenerateEncodedToken(user.UserName, identity);

            List<ApplicationPermissionViewModel> resources = new List<ApplicationPermissionViewModel>();
            if (roles != null)
            {
                var roleIds = roles.Select(x => (string)x.Id).ToList();
                //resources = db.Permissions.Include(x => x.Resource).Where(x => roleIds.Contains(x.RoleId) && x.IsAllowed).Select(x => (dynamic) new { name = x.Resource.Name, isAllowed = x.IsAllowed, isDisabled = x.IsDisabled })
                //    .ToList();

                resources = db.Permissions.Include(x => x.Resource).Include(x=>x.Role).Where(x => roleIds.Contains(x.RoleId) && x.IsAllowed).Select(x => new ApplicationPermissionViewModel(x))
                    .ToList();
                //resources = permissions.Select(x => x.name).ToList();
            }

            var jtiClaim = identity.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti);

            dynamic response = new
            {
                id = id,
                name = name,
                userName = user.UserName,
                roles = roles,
                access_token = token,
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                token_type = "bearer",
                resources = resources,
                jti = jtiClaim.Value
            };

            return response;
        }
    }
}