using DataModels.VM.UserRole;
using System.Collections.Generic;
using DataModels.VM.Company;
using DataModels.VM.Common;

namespace DataModels.VM.UserRolePermission
{
    public class UserRolePermissionFilterVM : CompanyFilterVM
    {
        public List<DropDownValues> ModuleList { get; set; }
        public int ModuleId { get; set; }
        public int UserRoleId { get; set; }

        public bool IsAllow { get; set; }
    }
}
