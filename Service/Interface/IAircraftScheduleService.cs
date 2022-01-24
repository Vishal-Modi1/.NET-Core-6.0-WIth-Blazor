using DataModels.VM.Common;
using DataModels.VM.Scheduler;

namespace Service.Interface
{
    public interface IAircraftScheduleService
    {
        CurrentResponse GetDetails(int roleId, int companyId);

        CurrentResponse Create(SchedulerVM schedulerVM);
    }
}
