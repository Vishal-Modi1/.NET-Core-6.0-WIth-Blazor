using DataModels.Entities;

namespace Repository.Interface
{
    public interface IRadarMapConfigurationRepository : IBaseRepository<RadarMapConfiguration>
    {
        void SetDefault(RadarMapConfiguration radarMapConfiguration);

        void SetDefault(long userId, short height, short width);
    }
}
