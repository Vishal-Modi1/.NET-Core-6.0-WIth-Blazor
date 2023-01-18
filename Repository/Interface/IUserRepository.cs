using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataModels.VM.User;
using DataModels.VM.Common;
using DataModels.VM.Account;
using DataModels.VM.UserPreference;

namespace Repository.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        bool IsEmailExist(string email);

        User Edit(User user);

        List<UserDataVM> List(UserDatatableParams datatableParams);

        void Delete(long id, long deletedBy);

        void UpdateActiveStatus(long id, bool isActive);

        bool ResetUserPassword(ResetPasswordVM resetPasswordVM);

        UserVM FindById(long id,bool isSuperAdmin, bool isInvited, int? companyId);

        bool UpdateImageName(long id, string imageName);

        List<UserPreferenceVM> FindPreferenceById(long id);

        List<DropDownLargeValues> ListDropdownValuesbyCompanyId(int companyId);

        List<User> ListAllbyCompanyId(int companyId);

        void UpdateArchiveStatus(long id, bool isArchive);
    }
}
