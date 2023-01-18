using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Weather;

namespace Service.Interface
{
    public interface IRadarMapConfigurationService
    {
        CurrentResponse FindByUserId(long userId);

        CurrentResponse SetDefault(RadarMapConfigurationVM radarMapConfigurationVM);
    }
}
