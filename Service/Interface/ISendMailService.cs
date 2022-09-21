using DataModels.VM.Common;
using DataModels.VM.User;

namespace Service.Interface
{
    public interface ISendMailService
    {
        bool NewUserAccountActivation(UserVM userVM, string token);
        
        CurrentResponse PasswordReset(string email, string url, string token);

        bool InviteUser(UserVM userVM, string token, long invitedUserId);
    }
}
