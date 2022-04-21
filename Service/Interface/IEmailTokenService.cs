using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IEmailTokenService 
    {
        EmailToken Create(EmailToken emailToken);

        CurrentResponse ValidateToken(string refreshToken, long userId);
    }
}
