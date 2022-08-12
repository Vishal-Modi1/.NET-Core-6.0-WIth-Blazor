using DataModels.Entities;
using DataModels.VM.Aircraft.AircraftStatusLog;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IAircraftStatusLogRepository : IBaseRepository<AircraftStatusLog>
    {
        AircraftStatusLog Edit(AircraftStatusLog aircraftStatusLog);

        void Delete(long id, long deletedBy);

        List<AircraftStatusLogDataVM> List(AircraftStatusLogDatatableParams datatableParams);
    }
}
