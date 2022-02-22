using DataModels.VM.MyAccount;

namespace Repository.Interface
{
    public interface IMyAccountRepository
    {
        bool ChangePassword(ChangePasswordVM changePasswordVM);

        bool IsValidOldPassword(string password, int userId);
    }
}
