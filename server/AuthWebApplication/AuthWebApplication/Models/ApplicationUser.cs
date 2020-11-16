using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AuthWebApplication.Models.Db;
using BizBook.Common.Library.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    public class ApplicationUser : IdentityUser, IIndex
    {
        [IsIndex]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [IsIndex]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        [IsIndex]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual ApplicationTenant Tenant { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string NewPassword { get; set; }

        [NotMapped]
        public string RoleId { get; set; }

        public void BuildIndices(ModelBuilder builder)
        {
            builder.BuildIndex<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasIndex(x => x.UserName).HasName("IX_UserName");
        }
    }
}