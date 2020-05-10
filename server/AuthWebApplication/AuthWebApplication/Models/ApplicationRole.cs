using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthWebApplication.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name) : base(name)
        {
        }

        [Column(TypeName = "varchar(64)")]
        public string Description { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string DefaultRoute { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string ShopId { get; set; }
    }
}