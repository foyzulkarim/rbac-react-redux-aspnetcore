using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BizBook.Common.Library.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApplication.Models
{
    [Table("AspNetTenants")]
    public class ApplicationTenant : IIndex
    {
        [IsIndex]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string Id { get; set; }

        [IsIndex]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; }

        [IsIndex]
        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public void BuildIndices(ModelBuilder builder)
        {
            builder.BuildIndex<ApplicationTenant>();
        }
    }
}