using DataModels.Entities;
using DataModels.VM;
using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IAircraftScheduleDetailService
    {
        CurrentResponse IsAircraftAlreadyCheckOut(long aircraftId);

        CurrentResponse CheckOut(AircraftScheduleDetailVM aircraftScheduleDetailVM);

        CurrentResponse CheckIn(List<AircraftEquipmentTimeVM> aircraftEquipmentsTimeList, long checkInBy);
    }
}
