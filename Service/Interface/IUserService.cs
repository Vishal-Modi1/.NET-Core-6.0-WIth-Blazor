﻿using DataModels.VM.User;
using DataModels.VM.Account;
using DataModels.VM.Common;
using DataModels.Entities;

namespace Service.Interface
{
    public interface IUserService
    {
        CurrentResponse Create(UserVM userVM);

        CurrentResponse GetDetails(long id, int companyId, int roleId);

        CurrentResponse IsEmailExist(string email);

        CurrentResponse Edit(UserVM userVM);

        CurrentResponse List(UserDatatableParams datatableParams);

        CurrentResponse Delete(long id, long deletedBy);

        CurrentResponse UpdateActiveStatus(long id, bool isActive);
        
        CurrentResponse ResetPassword(ResetPasswordVM resetPasswordVM);

        CurrentResponse GetFiltersValue(int roleId);

        CurrentResponse FindById(long id,bool isSuperAdmin, int? companyId);

        CurrentResponse UpdateImageName(long id, string imageName, int companyId);

        CurrentResponse ListDropdownValuesByCompanyId(int companyId);

        UserVM GetUserDetails(long id, int companyId, int roleId);

        CurrentResponse FindMyPreferencesById(long id);

        CurrentResponse GetMasterDetails(int roleId, bool isInvited, string token);

        CurrentResponse UpdateArchiveStatus(long id, bool isArchive);

        CurrentResponse GetById(long id, int companyId);
    }
}
