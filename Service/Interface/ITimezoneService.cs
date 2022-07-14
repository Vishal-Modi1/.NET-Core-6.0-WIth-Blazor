using DataModels.VM.Common;

namespace Service.Interface
{
    public interface ITimezoneService
    {
        CurrentResponse ListDropdownValues();
    }
}
