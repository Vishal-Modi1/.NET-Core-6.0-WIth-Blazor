using DataModels.VM;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftEquipementTimeService
    {
        CurrentResponse Create(AircraftEquipmentTimeVM aircraftEquipmentTimeVM);
        bool DeleteAllEquipmentTimeByAirCraftId(int AirCraftId,int UpdatedBy);
    }
}
