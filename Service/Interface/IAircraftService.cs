using DataModels.VM.Aircraft;
using DataModels.VM.Common;


namespace Service.Interface
{
    public interface IAircraftService
    {
        CurrentResponse Create(AirCraftVM airCraftVM);
        CurrentResponse List(AircraftFilterVM aircraftFilterVM);
        CurrentResponse Edit(AirCraftVM airCraftVM);
        CurrentResponse GetDetails(int id, int companyId);
        CurrentResponse Delete(int id);
        CurrentResponse UpdateImageName(int id, string imageName);
        CurrentResponse IsAirCraftExist(AirCraftVM airCraftVM);
        CurrentResponse GetFiltersValue(int companyId);

    }
}
