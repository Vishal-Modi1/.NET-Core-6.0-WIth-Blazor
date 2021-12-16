using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;


namespace Service.Interface
{
    public interface IAirCraftEquipmentService
    {
        CurrentResponse Create(AirCraftEquipmentsVM airCraftEquipmentsVM);
        CurrentResponse Edit(AirCraftEquipmentsVM airCraftEquipmentsVM);
        CurrentResponse List(int airCraftId);
        CurrentResponse Delete(int id);
        CurrentResponse Get(int id);
        CurrentResponse List(AircraftEquipmentDatatableParams datatableParams);

    }
}
