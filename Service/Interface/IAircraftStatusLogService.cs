using DataModels.VM.Aircraft.AircraftStatusLog;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftStatusLogService
    {
        CurrentResponse Create(AircraftStatusLogVM aircraftStatusLog);
        CurrentResponse List(AircraftStatusLogDatatableParams datatableParams);
        CurrentResponse Edit(AircraftStatusLogVM aircraftStatusLog);
        CurrentResponse Delete(int id, long deletedBy);
    }
}
