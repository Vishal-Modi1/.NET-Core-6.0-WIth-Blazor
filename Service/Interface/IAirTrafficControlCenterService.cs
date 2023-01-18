using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAirTrafficControlCenterService
    {
        CurrentResponse ListAll();
    }
}
