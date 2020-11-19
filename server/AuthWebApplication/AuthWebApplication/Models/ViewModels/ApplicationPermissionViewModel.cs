namespace AuthWebApplication.Models.ViewModels
{
    public static class Extensions
    {
        public static dynamic GetMinimalViewModel(this ApplicationPermission permission)
        {
            return new 
            {
                Name = permission.Resource.Name,
                IsAllowed = permission.IsAllowed.ToString(),
                IsDisabled = permission.IsDisabled.ToString()
            };
        }
    }

    public class ApplicationPermissionViewModel
    {
        public ApplicationPermissionViewModel()
        {

        }

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
            this.Name = permission.Resource.Name;
        }

        public string Id { get; set; }

        public string RoleId { get; set; }

        public string ResourceId { get; set; }

        public string RoleName { get; set; }

        public string ResourceName { get; set; }

        public string Name { get; set; }

        public string IsAllowed { get; set; }

        public string IsDisabled { get; set; }
    }
}
