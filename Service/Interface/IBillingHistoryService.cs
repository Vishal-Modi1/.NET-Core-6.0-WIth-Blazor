using DataModels.VM.BillingHistory;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IBillingHistoryService
    {
        CurrentResponse List(BillingHistoryDatatableParams datatableParams);
    }
}
