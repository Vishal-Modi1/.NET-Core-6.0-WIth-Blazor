using DataModels.VM.Common;
using DataModels.VM.Scheduler;

namespace Service.Interface
{
    public interface IAircraftScheduleService
    {
        CurrentResponse GetDetails(int roleId, int companyId, long id);

        CurrentResponse Create(SchedulerVM schedulerVM);

        CurrentResponse List(SchedulerFilter schedulerFilter);

        CurrentResponse Edit(SchedulerVM schedulerVM);

        CurrentResponse Delete(long id);
    }
}
