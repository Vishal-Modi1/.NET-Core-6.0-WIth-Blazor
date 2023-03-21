using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Weather;

namespace Service.Interface
{
    public interface INOAARadarMapConfigurationService
    {
        CurrentResponse FindByUserId(long userId);

        CurrentResponse SetDefault(NOAARadarMapConfigurationVM nOAARadarMapConfigurationVM);
    }
}
