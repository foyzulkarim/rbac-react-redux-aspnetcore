using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
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
            if (roles!=null)
            {
                var roleIds = roles.Select(x => (string)x.Id).ToList();
                var permissions = db.Permissions.Include(x => x.Resource).Where(x => roleIds.Contains(x.RoleId) && x.IsAllowed);
                var resources =
                    permissions.Select(x => new { name = x.Resource.Name, isAllowed = x.IsAllowed, isDisabled = x.IsDisabled })
                        .ToList();
            }
          
            var response = new
            {
                id = id,
                name = name,
                userName = user.UserName,
                //role = user.RoleName,
                //roleId = roleId,
                resources = "",
                roles = roles,
                access_token = token,
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                token_type = "bearer"
            };
            return response;
        }
    }
}