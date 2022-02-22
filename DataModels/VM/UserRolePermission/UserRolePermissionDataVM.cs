using System;

namespace DataModels.VM.UserRolePermission
{
    public class UserRolePermissionDataVM
    {
        public long Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int PermissionId { get; set; }
        public string PermissionType { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string DisplayName { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsAllowedForMobileApp { get; set; }
        public string Icon { get; set; }
        public int OrderNo { get; set; }
        public int TotalRecords { get; set; }
    }
}
