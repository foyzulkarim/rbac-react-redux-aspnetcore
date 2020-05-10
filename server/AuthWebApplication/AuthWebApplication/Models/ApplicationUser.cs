using System.ComponentModel.DataAnnotations.Schema;
using AuthWebApplication.Models.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "varchar(50)")]
        public override string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserExtension : IIndexBuilder<ApplicationUser>
    {
        public void BuildIndices(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasIndex(x => x.PhoneNumber).HasName("IX_PhoneNumber");

            builder.Entity<ApplicationUser>().HasIndex(x => x.FirstName).HasName("IX_FirstName");
            builder.Entity<ApplicationUser>().HasIndex(x => x.LastName).HasName("IX_LastName");
        }
    }
}