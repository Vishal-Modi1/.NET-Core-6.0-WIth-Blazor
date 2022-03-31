using DataModels.VM.BillingHistory;
using DataModels.VM.Common;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class BillingHistoryRepository : IBillingHistoryRepository
    {
        private MyContext _myContext;
        
        public List<BillingHistoryDataVM> List(DatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
                List<BillingHistoryDataVM> list;
                string sql = $"EXEC dbo.GetBillingHistoryList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length}," +
                    $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";

                list = _myContext.BillingHistoryList.FromSqlRaw<BillingHistoryDataVM>(sql).ToList();

                return list;
            }
        }
    }
}
