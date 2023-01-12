using DataModels.Entities;

namespace Repository.Interface
{
    public interface IWindyMapConfigurationRepository : IBaseRepository<WindyMapConfiguration>
    {
        void SetDefault(WindyMapConfiguration windyMapConfiguration);

        void SetDefault(long userId, short height, short width);
    }
}
