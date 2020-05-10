using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models.Db
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
        {

        }

        public SecurityDbContext()
        {

        }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationPermission> Permissions { get; set; }
        public DbSet<ApplicationResource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=BizBookIdentityDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.BuildIndices(this.GetType());

            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DbContextExtension
    {
        public static void BuildIndices(this ModelBuilder modelBuilder, Type dbContextType)
        {
            var assembly = Assembly.GetAssembly(dbContextType);
            if (assembly != null)
            {
                var allTypes = assembly.GetTypes().ToList();
                var types = allTypes.Where(x => x.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IIndexBuilder<>))).ToList();
                foreach (Type type in types)
                {
                    var methodInfo = type.GetMethods().FirstOrDefault(x => x.Name == "BuildIndices");
                    var classInstance = Activator.CreateInstance(type, null);
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(classInstance, new[] { modelBuilder });
                    }
                }
            }
        }
    }

    public interface IIndexBuilder<T> where T : class
    {
        void BuildIndices(ModelBuilder builder);
    }
}
