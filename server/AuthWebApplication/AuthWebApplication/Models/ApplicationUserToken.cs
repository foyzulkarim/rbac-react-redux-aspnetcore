using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BizBook.Common.Library.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    public class ApplicationUserToken : IdentityUserToken<string>, IIndex
    {
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual ApplicationTenant Tenant { get; set; }

        public void BuildIndices(ModelBuilder builder)
        {
            builder.BuildIndex<ApplicationUserToken>();
        }
    }
}