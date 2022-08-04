using System.Collections.Generic;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.UserRole;

namespace Repository.Interface
{
    public interface IUserRoleRepository
    {
        List<UserRoleVM> List();

        List<DropDownValues> ListDropDownValues(int roleId);

        UserRole FindById(int id);

        UserRole FindByUserIdAndCompanyId(long userId, int? companyId);
    }
}
