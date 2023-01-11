using DataModels.Entities;

namespace Repository.Interface
{
    public interface IAircraftLiveTrackerMapConfigurationRepository : IBaseRepository<AircraftLiveTrackerMapConfiguration>
    {
        void SetDefault(AircraftLiveTrackerMapConfiguration aircraftLiveTrackerMapConfiguration);
    }
}
