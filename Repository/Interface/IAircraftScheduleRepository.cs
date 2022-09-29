using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftScheduleRepository
    {
        #region ActivityType

        List<DropDownLargeValues> ListActivityTypeDropDownValues(int roleId);

        #endregion

        List<SchedulerVM> List(SchedulerFilter schedulerFilter);

        AircraftSchedule Create(AircraftSchedule aircraftSchedule);

        AircraftSchedule FindByCondition(Expression<Func<AircraftSchedule, bool>> predicate);

        AircraftSchedule Edit(AircraftSchedule aircraftSchedule);

        void Delete(long id, long deletedBy);

        void EditEndTime(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM);

        bool IsAircraftAvailable(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM);
    }
}
