using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthWebApplication.Models
{
    [Table("AspNetResources")]
    public class ApplicationResource
    {
        public ApplicationResource()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Column(TypeName = "varchar(128)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public ResourceType ResourceType { get; set; }

        public virtual ICollection<ApplicationPermission> Permissions { get; set; }
    }
}