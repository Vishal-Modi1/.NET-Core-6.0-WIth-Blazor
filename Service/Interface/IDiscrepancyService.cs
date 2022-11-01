using DataModels.VM.Common;
using DataModels.VM.Discrepancy;

namespace Service.Interface
{
    public interface IDiscrepancyService
    {
        CurrentResponse Create(DiscrepancyVM discrepancyVM);
        CurrentResponse Edit(DiscrepancyVM discrepancyVM);
        CurrentResponse List(DiscrepancyDatatableParams datatableParams);
        CurrentResponse GetDetails(long id);
    }
}
