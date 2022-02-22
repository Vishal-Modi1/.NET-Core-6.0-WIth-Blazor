using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftModelService
    {
        CurrentResponse Create(AircraftModel aircraftModel);

        CurrentResponse List();
    }
}
