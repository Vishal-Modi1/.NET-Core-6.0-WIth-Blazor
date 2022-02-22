using DataModels.Entities;
using System.Collections.Generic;
using DataModels.VM.UserRolePermission;


namespace Repository.Interface
{
    public interface IUserRolePermissionRepository
    {
        List<UserRolePermissionDataVM> GetByRoleId(int roleId, int? companyId);

        UserRolePermission Update(UserRolePermissionDataVM  userRolePermissionVM);

        List<UserRolePermissionDataVM> List(UserRolePermissionDatatableParams datatableParams);

        void UpdatePermission(int id, bool isAllow);

        void UpdatePermissions(UserRolePermissionFilterVM userRolePermissionFilterVM);

        void UpdateMobileAppPermission(int id, bool isAllow);

        void UpdateMobileAppPermissions(UserRolePermissionFilterVM userRolePermissionFilterVM);
    }
}
