using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class DiscrepancyRepository : BaseRepository<Discrepancy>, IDiscrepancyRepository
    {
        private readonly MyContext _myContext;

        public DiscrepancyRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public Discrepancy Edit(Discrepancy discrepancy)
        {
            Discrepancy existingDiscrepancy = _myContext.Discrepancies.Where(p => p.Id == discrepancy.Id).FirstOrDefault();

            if (existingDiscrepancy != null)
            {
                existingDiscrepancy.AircraftId = discrepancy.AircraftId;
                existingDiscrepancy.ActionTaken = discrepancy.ActionTaken;
                existingDiscrepancy.Description = discrepancy.Description;
                existingDiscrepancy.DiscrepancyStatusId = discrepancy.DiscrepancyStatusId;
                existingDiscrepancy.ReportedByUserId = discrepancy.ReportedByUserId;
                existingDiscrepancy.CompanyId = discrepancy.CompanyId;

                existingDiscrepancy.UpdatedBy = discrepancy.UpdatedBy;
                existingDiscrepancy.UpdatedOn = discrepancy.UpdatedOn;
                
                existingDiscrepancy.IsActive = discrepancy.IsActive;
                existingDiscrepancy.IsDeleted = discrepancy.IsDeleted;

                _myContext.SaveChanges();
            }

            return discrepancy;
        }

        public List<DiscrepancyDataVM> List(DiscrepancyDatatableParams datatableParams)
        {
            List<DiscrepancyDataVM> list;

            string sql = $"EXEC dbo.GetDiscrepanciesList '{datatableParams.SearchText }', { datatableParams.Start }, " +
                $"{datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', " +
                $"{datatableParams.CompanyId}, {datatableParams.AircraftId}";

            list = _myContext.DiscrepancyDataVM.FromSqlRaw<DiscrepancyDataVM>(sql).ToList();

            return list;
        }
    }
}
