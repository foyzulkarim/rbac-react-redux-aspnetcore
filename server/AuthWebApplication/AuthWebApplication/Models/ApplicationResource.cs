using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BizBook.Common.Library.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    [Table("AspNetResources")]
    public class ApplicationResource : IIndex
    {
        public ApplicationResource()
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
        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public ResourceType ResourceType { get; set; }

        public virtual ICollection<ApplicationPermission> Permissions { get; set; }

        public void BuildIndices(ModelBuilder builder)
        {
            builder.BuildIndex<ApplicationResource>();
        }
    }
}