using DataModels.Entities;
using DataModels.VM;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftScheduleDetailService
    {
        CurrentResponse IsAircraftAlreadyCheckOut(long aircraftId);

        CurrentResponse Create(AircraftScheduleDetailVM aircraftScheduleDetailVM);
    }
}
