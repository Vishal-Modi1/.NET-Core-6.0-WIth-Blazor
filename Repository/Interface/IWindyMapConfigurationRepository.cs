using DataModels.Entities;

namespace Repository.Interface
{
    public interface IWindyMapConfigurationRepository : IBaseRepository<WindyMapConfiguration>
    {
        void SetDefault(WindyMapConfiguration windyMapConfiguration);
    }
}
