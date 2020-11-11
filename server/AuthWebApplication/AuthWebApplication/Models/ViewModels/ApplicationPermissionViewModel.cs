using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthWebApplication.Models.ViewModels
{
    public class ApplicationPermissionViewModel
    {
        public ApplicationPermissionViewModel(ApplicationPermission permission)
        {
            this.Id = permission.Id;
            this.RoleId = permission.RoleId;
            this.ResourceId = permission.ResourceId;
            this.RoleId = permission.RoleId;
            this.RoleName = permission.Role.Name;
            this.ResourceName = permission.Resource.Name;
            this.IsAllowed = permission.IsAllowed.ToString();
            this.IsDisabled = permission.IsDisabled.ToString();
        }

        public string Id { get; set; }

        public string RoleId { get; set; }

        public string ResourceId { get; set; }

        public string RoleName { get; set; }

        public string ResourceName { get; set; }

        public string IsAllowed { get; set; }

        public string IsDisabled { get; set; }
    }
}
