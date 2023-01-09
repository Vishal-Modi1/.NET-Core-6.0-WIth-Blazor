using DataModels.VM.Common;
using DataModels.VM.Scheduler;

namespace Service.Interface
{
    public interface IAircraftScheduleService
    {
        CurrentResponse GetDetails(int roleId, int companyId, long id, long userId);
        
        CurrentResponse GetDropdownValuesByCompanyId(int roleId, int companyId, long userId);

        CurrentResponse Create(SchedulerVM schedulerVM, string timezone);

        CurrentResponse List(SchedulerFilter schedulerFilter);

        CurrentResponse Edit(SchedulerVM schedulerVM, string timezone);

        CurrentResponse Delete(long id, long deletedBy);

        CurrentResponse EditEndTime(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM);

        CurrentResponse ListActivityTypeDropDownValues(int roleId);

    }
}
