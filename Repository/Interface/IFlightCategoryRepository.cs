using DataModels.Entities;
using System.Collections.Generic;
using DataModels.VM.Common;

namespace Repository.Interface
{
    public interface IFlightCategoryRepository : IBaseRepository<FlightCategory>
    {
        List<DropDownValues> ListDropDownValuesByCompanyId(int companyId);

        FlightCategory Edit(FlightCategory flightCategory);

        List<FlightCategory> ListByCompanyId(int companyId);

        void Delete(int id);

        void CreateForAllCompanies(FlightCategory flightCategory);
    }
}
