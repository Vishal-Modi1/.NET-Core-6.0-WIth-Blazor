using DataModels.VM.Scheduler;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IFlightCategoryService
    {
        CurrentResponse Create(FlightCategoryVM flightCategoryVM);
        CurrentResponse ListDropDownValues(int companyId);
        CurrentResponse Edit(FlightCategoryVM flightCategoryVM);
        CurrentResponse Delete(int id);
        CurrentResponse ListByCompanyId(int companyId);
    }
}
