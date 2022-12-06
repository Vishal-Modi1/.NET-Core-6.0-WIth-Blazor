using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IWindyMapConfigurationService
    {
        CurrentResponse FindByUserId(long userId);

        CurrentResponse SetDefault(WindyMapConfiguration WindyMapConfiguration);
    }
}
