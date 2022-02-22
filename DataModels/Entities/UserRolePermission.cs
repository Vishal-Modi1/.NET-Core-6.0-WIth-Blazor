using System;

namespace DataModels.Entities
{
    public class UserRolePermission
    {
        public long Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int ModuleId { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsAllowedForMobileApp { get; set; }

        public Nullable<int> CompanyId { get; set; }
    }
}
