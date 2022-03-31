using DataModels.VM.Common;
using System.Collections.Generic;
using DataModels.VM.BillingHistory;

namespace Repository.Interface
{
    public interface IBillingHistoryRepository
    {
        List<BillingHistoryDataVM> List(DatatableParams datatableParams);
    }
}
