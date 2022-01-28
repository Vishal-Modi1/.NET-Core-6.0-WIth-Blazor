using DataModels.Entities;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IAircraftScheduleHobbsTimeRepository
    {
        List<AircraftScheduleHobbsTime> Create(List<AircraftScheduleHobbsTime> aircraftScheduleHobbsTimesList);
    }
}
