using DataModels.Entities;
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

        public List<BillingHistoryDataVM> List(BillingHistoryDatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                List<BillingHistoryDataVM> list;

                string sql = $"EXEC dbo.GetBillingHistoryList '{ datatableParams.SearchText }', { datatableParams.Start }, {datatableParams.Length}," +
                    $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";

                if(datatableParams.CompanyId != 0)
                {
                    sql += $",{ datatableParams.CompanyId}";
                }

                if(datatableParams.UserId != 0)
                {
                    sql += $",{ datatableParams.UserId}";

                }

                list = _myContext.BillingHistoryList.FromSqlRaw<BillingHistoryDataVM>(sql).ToList();

                return list;
            }
        }

        public BillingHistory Create(BillingHistory billingHistory)
        {
            using (_myContext = new MyContext())
            {
                billingHistory.Id = System.Guid.NewGuid();
                _myContext.BillingHistories.Add(billingHistory);

                _myContext.SaveChanges();

                return billingHistory;
            }
        }

        //public bool IsPlanAlreadyInUse(int planId)
        //{
        //    using (_myContext = new MyContext())
        //    {
        //        _myContext.BillingHistories.Any(p=> p.pl)

        //        return billingHistory;
        //    }
        //}

        // the implentatation of the campus tension the tension if the task the implementation
        // the tank of the task of
        // templates 
            
    }
}
