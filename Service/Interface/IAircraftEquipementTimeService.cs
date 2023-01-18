using DataModels.Entities;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IAircraftEquipementTimeService
    {
        CurrentResponse Create(AircraftEquipmentTimeCreateVM aircraftEquipmentTimeVM);
        bool DeleteAllEquipmentTimeByAircraftId(long AircraftId, long UpdatedBy);
        List<AircraftEquipmentTime> ToDataObjectList(List<AircraftEquipmentTimeVM> aircraftEquipmentTimesVMList);
        List<AircraftEquipmentTimeCreateVM> ToCreateBusinessObjectList(List<AircraftEquipmentTime> aircraftEquipmentTimesList);
    }
}
