using DataModels.VM.Common;

namespace DataModels.VM.UserRolePermission
{
    public class UserRolePermissionDatatableParams : DatatableParams
    {
        public int ModuleId { get; set; }

        public int RoleId { get; set; }
    }
}
