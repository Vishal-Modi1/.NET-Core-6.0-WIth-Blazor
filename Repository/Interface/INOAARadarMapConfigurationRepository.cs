using DataModels.Entities;

namespace Repository.Interface
{
    public interface INOAARadarMapConfigurationRepository : IBaseRepository<NOAARadarMapConfiguration>
    {
        void SetDefault(NOAARadarMapConfiguration nOAARadarMapConfiguration);

        void SetDefault(long userId, short height, short width);
    }
}
