using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

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
        public string ShopId { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }
    }
}