using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthLibrary.Services;
using AuthWebApplication.Models;
using AuthWebApplication.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthWebApplication.Utilities
{
    public class Tokens
    {
        public static async Task<object> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory,
            JwtIssuerOptions jwtOptions, ApplicationUser user, List<string> roles, List<dynamic> resources2)
        {
            string id = identity.Claims.Single(c => c.Type == "id").Value;
            var name = user.FirstName + " " + user.LastName;
            string token = await jwtFactory.GenerateEncodedToken(user.UserName, identity);

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
                resources = resources2,
                jti = jtiClaim.Value
            };

            return response;
        }
    }
}