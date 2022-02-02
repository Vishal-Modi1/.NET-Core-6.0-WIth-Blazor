using DataModels.Entities;
using System;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftScheduleDetailRepository
    {
        bool IsAircraftAlreadyCheckOut(long aircraftId);

        AircraftScheduleDetail CheckOut(AircraftScheduleDetail aircraftScheduleDetail);

        AircraftScheduleDetail FindByCondition(Expression<Func<AircraftScheduleDetail, bool>> predicate);

        AircraftScheduleDetail CheckIn(long checkInBy, DateTime checkInTime, long aircraftScheduleId);

        AircraftScheduleDetail UnCheckOut(long id);
    }
}
