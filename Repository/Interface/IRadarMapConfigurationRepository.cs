using DataModels.Entities;

namespace Repository.Interface
{
    public interface IRadarMapConfigurationRepository : IBaseRepository<RadarMapConfiguration>
    {
        void SetDefault(RadarMapConfiguration radarMapConfiguration);
    }
}
