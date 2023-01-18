using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;


namespace Service.Interface
{
    public interface IAircraftEquipmentService
    {
        CurrentResponse Create(AircraftEquipmentsVM airCraftEquipmentsVM);
        CurrentResponse Edit(AircraftEquipmentsVM airCraftEquipmentsVM);
        CurrentResponse List(int airCraftId);
        CurrentResponse Delete(int id, long deletedBy);
        CurrentResponse Get(int id);
        CurrentResponse List(AircraftEquipmentDatatableParams datatableParams);

    }
}
