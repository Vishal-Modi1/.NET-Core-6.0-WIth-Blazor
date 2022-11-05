using DataModels.Entities;
using DataModels.VM.Discrepancy;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class DiscrepancyHistoryRepository : BaseRepository<DiscrepancyHistory>, IDiscrepancyHistoryRepository
    {
        private readonly MyContext _myContext;

        public DiscrepancyHistoryRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public List<DiscrepancyHistoryVM> List(long discrepancyId)
        {
            string sql = $"EXEC dbo.FindDiscrepanciesHistoryByDiscrepancyId {discrepancyId } ";
            List<DiscrepancyHistoryVM> list = _myContext.DiscrepancyHistoryVM.FromSqlRaw<DiscrepancyHistoryVM>(sql).ToList();

            return list;
        }
    }
}
