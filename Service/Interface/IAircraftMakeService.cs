using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftMakeService
    {
        CurrentResponse Create(AircraftMake aircraftMake);

        CurrentResponse List();
    }
}
