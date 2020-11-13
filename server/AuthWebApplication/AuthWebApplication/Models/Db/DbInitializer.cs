using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AuthWebApplication.Models.Db
{
    public class DbInitializer
    {
        private const string SuperAdmin = "SuperAdmin";

        public static void Initialize(SecurityDbContext context, UserManager<ApplicationUser> userManager,
            string password, ILogger<DbInitializer> dbInitializerLogger)
        {
            CreateRole(context, dbInitializerLogger, SuperAdmin);
            CreateDefaultUser(context, userManager, dbInitializerLogger, "Super Admin", "foyzulkarim@gmail.com",
                    password, SuperAdmin).GetAwaiter()
                .GetResult();
            CreateResources(context, dbInitializerLogger);
            CreatePermissions(context, dbInitializerLogger);
        }

        private static void CreateRole(SecurityDbContext context, ILogger<DbInitializer> logger, string role)
        {
            logger.LogInformation($"Create the role `{role}` for application");
            var any = context.ApplicationRoles.AsEnumerable()
                .Any(x => string.Equals(x.Name, role, StringComparison.CurrentCultureIgnoreCase));
            if (!any)
            {
                var appRole = new ApplicationRole(role);
                context.ApplicationRoles.Add(appRole);
                var i = context.SaveChanges();
                if (i > 0)
                {
                    logger.LogDebug($"Created the role `{role}` successfully");
                }
                else
                {
                    ApplicationException exception =
                        new ApplicationException($"Default role `{role}` cannot be created");
                    logger.LogError(exception, $"Exception occurred. {exception.Message}");
                    throw exception;
                }
            }
        }

        private static async Task<ApplicationUser> CreateDefaultUser(SecurityDbContext context,
            UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string displayName, string email,
            string password, string role)
        {
            logger.LogInformation($"Create default user with email `{email}` for application");
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = displayName,
                    IsActive = true
                };

                IdentityResult userCreateResult = await userManager.CreateAsync(user, password);

                if (userCreateResult.Succeeded)
                {
                    logger.LogDebug($"Created default user `{email}` successfully");
                }
                else
                {
                    ApplicationException exception =
                        new ApplicationException($"Default user `{email}` cannot be created");
                    logger.LogError(exception, $"Exception occurred. {exception.Message}");
                    throw exception;
                }
            }

            AddRoleToApplicationUser(context, logger, user, role);
            return user;
        }

        private static void AddRoleToApplicationUser(SecurityDbContext context, ILogger<DbInitializer> logger,
            ApplicationUser user, string role)
        {
            var applicationRole = context.ApplicationRoles.First(x => x.Name == role);
            var userRole =
                context.ApplicationUserRoles.FirstOrDefault(x => x.UserId == user.Id && x.RoleId == applicationRole.Id);
            if (userRole == null)
            {
                ApplicationUserRole entity = new ApplicationUserRole() { RoleId = applicationRole.Id, UserId = user.Id };
                context.ApplicationUserRoles.Add(entity);
                var saveChanges = context.SaveChanges();
                if (saveChanges == 0)
                {
                    ApplicationException exception =
                        new ApplicationException($"Adding role to user `{user.Email}` cannot be done");
                    logger.LogError(exception, $"Exception occurred. {exception.Message}");
                    throw exception;
                }
            }
        }

        private static void CreateResources(SecurityDbContext context, ILogger<DbInitializer> logger)
        {
            var readAllText = File.ReadAllText("./Resources/resources.json");
            var resources = JsonConvert.DeserializeObject<List<ApplicationResource>>(readAllText);
            foreach (var resource in resources)
            {
                logger.LogInformation($"Create the resource `{resource.Name}` for application");
                var any = context.Resources.AsEnumerable()
                    .Any(x => string.Equals(x.Name, resource.Name, StringComparison.CurrentCultureIgnoreCase));
                if (!any)
                {
                    context.Resources.Add(resource);
                    var i = context.SaveChanges();
                    if (i > 0)
                    {
                        logger.LogDebug($"Created the resource `{resource.Name}` successfully");
                    }
                    else
                    {
                        ApplicationException exception =
                            new ApplicationException($"Default resource `{resource.Name}` cannot be created");
                        logger.LogError(exception, $"Exception occurred. {exception.Message}");
                        throw exception;
                    }
                }
            }
        }

        private static void CreatePermissions(SecurityDbContext context, ILogger<DbInitializer> logger)
        {
            var readAllText = File.ReadAllText("./Resources/permissions.json");
            var permissions = JsonConvert.DeserializeObject<List<ApplicationPermission>>(readAllText);
            foreach (var permission in permissions)
            {
                logger.LogInformation($"Create the permission for resource `{permission.Resource.Name}` and role `{permission.Role.Name} ");

                var role = context.ApplicationRoles.FirstOrDefault(x=>x.Name == permission.Role.Name);
                var resource = context.Resources.FirstOrDefault(x=>x.Name == permission.Resource.Name);

                var any = context.Permissions.Include(x => x.Role).Include(x => x.Resource).AsEnumerable()
                    .Any(x => string.Equals(x.RoleId, role.Id, StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(x.ResourceId, resource.Id, StringComparison.CurrentCultureIgnoreCase));
                if (!any)
                {
                    var appPermission = new ApplicationPermission(){
                        IsAllowed = permission.IsAllowed,
                        ResourceId = resource.Id,
                        RoleId = role.Id 
                    };
                    context.Permissions.Add(appPermission);
                    var i = context.SaveChanges();
                    if (i > 0)
                    {
                        logger.LogDebug($"Created the permission for resource `{permission.Resource.Name}` and role `{permission.Role.Name}");
                    }
                    else
                    {
                        ApplicationException exception =
                            new ApplicationException($"Default resource `{permission.Resource.Name}` & role `{permission.Role.Name} cannot be created");
                        logger.LogError(exception, $"Exception occurred. {exception.Message}");
                        throw exception;
                    }
                }
            }
        }
    }
}