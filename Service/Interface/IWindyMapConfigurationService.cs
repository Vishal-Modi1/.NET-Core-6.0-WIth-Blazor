using DataModels.VM.Common;
using DataModels.VM.Weather;

namespace Service.Interface
{
    public interface IWindyMapConfigurationService
    {
        CurrentResponse FindByUserId(long userId);

        CurrentResponse SetDefault(WindyMapConfigurationVM windyMapConfigurationVM);
    }
}
