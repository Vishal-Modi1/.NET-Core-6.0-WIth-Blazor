using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftStatusService
    {
        CurrentResponse ListDropDownValues();

        CurrentResponse ListAll();

        CurrentResponse GetById(byte id);
    }
}
