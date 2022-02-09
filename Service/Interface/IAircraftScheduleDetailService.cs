using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;
using System.Collections.Generic;
using DataModels.VM.Scheduler;

namespace Service.Interface
{
    public interface IAircraftScheduleDetailService
    {
        CurrentResponse IsAircraftAlreadyCheckOut(long aircraftId);

        CurrentResponse CheckOut(AircraftSchedulerDetailsVM aircraftScheduleDetailVM);
        CurrentResponse UnCheckOut(long id);

        CurrentResponse CheckIn(List<AircraftEquipmentTimeVM> aircraftEquipmentsTimeList, long checkInBy);
    }
}
