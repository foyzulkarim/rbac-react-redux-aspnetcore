using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthWebApplication.Models
{
    [Table("AspNetPermissions")]
    public class ApplicationPermission
    {
        public ApplicationPermission()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Column(TypeName = "varchar(128)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string ResourceId { get; set; }


        public string RoleId { get; set; }

        public bool IsAllowed { get; set; }

        public bool IsDisabled { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string ShopId { get; set; }

        [ForeignKey("ResourceId")]
        public virtual ApplicationResource Resource { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }
    }
}