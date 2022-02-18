using DataModels.VM.Common;
using DataModels.VM.UserPreference;

namespace Service.Interface
{
    public interface IUserPreferenceService
    {
        CurrentResponse Create(UserPreferenceVM userPreferenceVM);
    }
}
