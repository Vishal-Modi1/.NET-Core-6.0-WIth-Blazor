using DataModels.VM.UserRolePermission;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IUserRolePermissionService
    {
        CurrentResponse GetByRoleId(int roleId, int? companyId);

        CurrentResponse List(UserRolePermissionDatatableParams datatableParams);

        CurrentResponse UpdatePermission(int id, bool isAllow);

        CurrentResponse UpdateFullPermission(int id, bool isAllow);

        CurrentResponse GetFiltersValue(int roleId);

        CurrentResponse UpdateMultiplePermissions(UserRolePermissionFilterVM userRolePermissionFilterVM);
    }
}
