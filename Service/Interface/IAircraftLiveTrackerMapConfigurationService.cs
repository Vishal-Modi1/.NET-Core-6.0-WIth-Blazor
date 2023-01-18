using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Weather;

namespace Service.Interface
{
    public interface IAircraftLiveTrackerMapConfigurationService
    {
        CurrentResponse FindByUserId(long userId);

        CurrentResponse SetDefault(AircraftLiveTrackerMapConfigurationVM aircraftLiveTrackerMapConfigurationVM);
    }
}
