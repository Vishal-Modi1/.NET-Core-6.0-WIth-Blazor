using DataModels.Entities;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IAircraftScheduleRepository
    {
        #region ActivityType

        List<DropDownValues> ListActivityTypeDropDownValues(int roleId);

        #endregion
        
        AircraftSchedule Create(AircraftSchedule aircraftSchedule);
    }
}
