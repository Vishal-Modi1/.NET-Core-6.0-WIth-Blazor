using DataModels.VM.Account;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAccountService
    {
        CurrentResponse GetValidUser(LoginVM loginVM);

        CurrentResponse ActivateAccount(string token);
    }
}
