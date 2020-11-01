using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            using (var scope = host.Services.CreateScope()) // https://stackoverflow.com/a/51692055/326597
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SecurityDbContext>();
                    context.Database.Migrate();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var dbInitializerLogger = services.GetRequiredService<ILogger<DbInitializer>>();
                    var config = host.Services.GetRequiredService<IConfiguration>();
                    var pass = config["pass"];
                    DbInitializer.Initialize(context, userManager, pass, dbInitializerLogger);
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred while migrating the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
