using DataModels.VM.User;
using DataModels.VM.Account;
using DataModels.VM.Common;

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

        CurrentResponse FindById(long id);

        CurrentResponse UpdateImageName(long id, string imageName);

        CurrentResponse ListDropDownValuesByCompanyId(int companyId);

        UserVM GetUserDetails(long id, int companyId, int roleId);

        CurrentResponse FindMyPreferencesById(long id);

        CurrentResponse GetMasterDetails(int roleId);
    }
}
