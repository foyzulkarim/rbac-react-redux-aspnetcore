using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BizBook.Common.Library.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    [Table("AspNetPermissions")]
    public class ApplicationPermission : IIndex
    {
        public ApplicationPermission()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string Id { get; set; }

        [IsIndex]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string ResourceId { get; set; }

        public string RoleId { get; set; }

        public string UserId { get; set; }

        public bool IsAllowed { get; set; }

        public bool IsDisabled { get; set; }

        [ForeignKey("ResourceId")]
        public virtual ApplicationResource Resource { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public void BuildIndices(ModelBuilder builder)
        {
            builder.BuildIndex<ApplicationPermission>();
        }
    }
}