using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftClassService
    {
        CurrentResponse Create(AircraftClass aircraftClass);

        CurrentResponse List();
    }
}
