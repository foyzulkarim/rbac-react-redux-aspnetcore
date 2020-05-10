using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
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
           // string roleId = identity.Claims.Single(c => c.Type == "roleId").Value;
            string id = identity.Claims.Single(c => c.Type == "id").Value;
            var name = user.FirstName + " " + user.LastName;
            string token = await jwtFactory.GenerateEncodedToken(user.UserName, identity);

            //IQueryable<ApplicationPermission> permissions = db.Permissions.Where(x => x.RoleId == roleId && x.IsAllowed);
            //var resources =
            //    permissions.Select(x => new { name = x.Resource.Name, isAllowed = x.IsAllowed, isDisabled = x.IsDisabled })
            //        .ToList();
            // string allowedResources = JsonConvert.SerializeObject(resources);

            var response = new
            {
                id = id,
                name = name,
                userName = user.UserName,
                role = user.RoleName,
                //roleId = roleId,
                //resources = allowedResources,
                access_token = token,
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                token_type = "bearer"
            };
            return response;
        }
    }
}