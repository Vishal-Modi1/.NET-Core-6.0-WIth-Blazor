using DataModels.VM.MyAccount;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IMyAccountService
    {
        CurrentResponse ChangePassword(ChangePasswordVM changePasswordVM);

        CurrentResponse GetMyProfileDetails(int companyId, int roleId, long userId);
    }
}
