using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IFlightCategoryService
    {
        CurrentResponse Create(FlightCategory flightCategory);
        CurrentResponse ListDropDownValues(int companyId);
        CurrentResponse Edit(FlightCategory flightCategory);
        CurrentResponse Delete(int id);
        CurrentResponse ListByCompanyId(int companyId);
    }
}
