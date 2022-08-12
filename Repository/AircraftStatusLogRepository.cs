using DataModels.Entities;
using DataModels.VM.Aircraft.AircraftStatusLog;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class AircraftStatusLogRepository : BaseRepository<AircraftStatusLog>, IAircraftStatusLogRepository
    {
        private readonly MyContext _myContext;

        public AircraftStatusLogRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public AircraftStatusLog Edit(AircraftStatusLog aircraftStatusLog)
        {
            //AircraftStatusLog existingLog = _myContext.AircraftStatusLogs.Where(p => p.Id == aircraftStatusLog.Id).FirstOrDefault();

            //if (existingLog != null)
            //{
            //    existingLog.AircraftStatusId = aircraftStatusLog.AircraftStatusId;
            //    existingLog.Reason = aircraftStatusLog.Reason;

            //    existingLog.UpdatedBy = aircraftStatusLog.UpdatedBy;
            //    existingLog.UpdatedOn = aircraftStatusLog.UpdatedOn;

            //    _myContext.SaveChanges();
            //}

            return aircraftStatusLog;
        }

        public void Delete(long id, long deletedBy)
        {
            //AircraftStatusLog aircraftStatusLog = _myContext.AircraftStatusLogs.Where(p => p.Id == id).FirstOrDefault();

            //if (aircraftStatusLog != null)
            //{
            //    aircraftStatusLog.IsDeleted = true;
            //    aircraftStatusLog.DeletedBy = deletedBy;
            //    aircraftStatusLog.DeletedOn = DateTime.UtcNow;

            //    _myContext.SaveChanges();
            //}
        }

        public List<AircraftStatusLogDataVM> List(AircraftStatusLogDatatableParams datatableParams)
        {
            //int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
            //List<AircraftStatusLogDataVM> list;

            //string sql = $"EXEC dbo.GetAircraftStatusLogsList '{ datatableParams.SearchText }'" +
            //    $", { pageNo }, {datatableParams.Length},'{datatableParams.SortOrderColumn}'" +
            //    $",'{datatableParams.OrderType}', {datatableParams.AircraftId}";

            //list = _myContext.AircraftStatusLogsDataVM.FromSqlRaw<AircraftStatusLogDataVM>(sql).ToList();

            //return list;

            return null;
        }
    }
}
