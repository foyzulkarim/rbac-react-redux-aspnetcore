using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BizBook.Common.Library.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>, IIndex
    {
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual ApplicationTenant Tenant { get; set; }

        [NotMapped]
        public virtual ApplicationUser User { get; set; }

        [NotMapped]
        public virtual ApplicationRole Role { get; set; }

        public void BuildIndices(ModelBuilder builder)
        {
            builder.BuildIndex<ApplicationUserRole>();
        }
    }
}