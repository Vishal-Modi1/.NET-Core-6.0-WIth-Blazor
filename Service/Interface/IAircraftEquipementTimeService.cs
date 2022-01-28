using DataModels.Entities;
using DataModels.VM;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IAircraftEquipementTimeService
    {
        CurrentResponse Create(AircraftEquipmentTimeVM aircraftEquipmentTimeVM);
        bool DeleteAllEquipmentTimeByAirCraftId(long AirCraftId,int UpdatedBy);

        List<AircraftEquipmentTime> ToDataObjectList(List<AircraftEquipmentTimeVM> aircraftEquipmentTimesVMList);
    }
}
