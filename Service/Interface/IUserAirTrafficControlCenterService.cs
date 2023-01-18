using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IUserAirTrafficControlCenterService
    {
        CurrentResponse FindByUserId(long userId);

        CurrentResponse SetDefault(UserAirTrafficControlCenter userAirTrafficControlCenter);
    }
}
