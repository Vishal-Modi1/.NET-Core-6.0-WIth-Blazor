using DataModels.Entities;
using System;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftScheduleDetailRepository
    {
        bool IsAircraftAlreadyCheckOut(long aircraftId);

        AircraftScheduleDetail Create(AircraftScheduleDetail aircraftScheduleDetail);

        AircraftScheduleDetail FindByCondition(Expression<Func<AircraftScheduleDetail, bool>> predicate);
    }
}
