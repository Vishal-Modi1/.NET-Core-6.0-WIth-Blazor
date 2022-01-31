using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftScheduleHobbsTimeRepository
    {
        List<AircraftScheduleHobbsTime> Create(List<AircraftScheduleHobbsTime> aircraftScheduleHobbsTimesList);

        List<AircraftScheduleHobbsTime> ListByCondition(Expression<Func<AircraftScheduleHobbsTime, bool>> predicate);
    }
}
