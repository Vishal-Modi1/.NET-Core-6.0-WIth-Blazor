using DataModels.VM.Aircraft;
using DataModels.VM.Common;


namespace Service.Interface
{
    public interface IAircraftService
    {
        CurrentResponse Create(AircraftVM airCraftVM);
        CurrentResponse List(AircraftDatatableParams datatableParams);
        CurrentResponse ListAllByCompanyId(int companyId);
        CurrentResponse Edit(AircraftVM airCraftVM);
        CurrentResponse GetDetails(long id, int companyId);
        CurrentResponse Delete(long id, long deletedBy);
        CurrentResponse UpdateImageName(long id, string imageName);
        CurrentResponse IsAircraftExist(long id, string tailNo);
        CurrentResponse GetFiltersValue(int companyId);
        CurrentResponse ListAircraftDropdownValues(int companyId);
        CurrentResponse UpdateStatus(long id, byte statusId);
        CurrentResponse LockAircraft(long id, bool isLock);
    }
}
