using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public  class AircraftScheduleDetailRepository : IAircraftScheduleDetailRepository
    {
        private MyContext _myContext;

        public bool IsAircraftAlreadyCheckOut(long aircraftId)
        {
            using (_myContext = new MyContext())
            {
                bool isAlreadyCheckOut = (from aircraftSchedule in _myContext.AircraftSchedules
                                          join aircraftScheduleDetails in _myContext.AircraftScheduleDetails
                                          on aircraftSchedule.Id equals aircraftScheduleDetails.AircraftScheduleId
                                          where aircraftSchedule.AircraftId == aircraftId
                                          && aircraftScheduleDetails.IsCheckOut == true
                                          select new { Id = aircraftSchedule.Id }).Count() > 0;

                return isAlreadyCheckOut;
            }
        }

        public AircraftScheduleDetail Create(AircraftScheduleDetail aircraftScheduleDetail)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftScheduleDetails.Add(aircraftScheduleDetail);
                _myContext.SaveChanges();

                return aircraftScheduleDetail;
            }
        }

        public AircraftScheduleDetail FindByCondition(Expression<Func<AircraftScheduleDetail, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftScheduleDetails.Where(predicate).FirstOrDefault();
            }
        }
    }
}
