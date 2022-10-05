using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class AircraftScheduleDetailRepository : IAircraftScheduleDetailRepository
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
                                          && aircraftSchedule.IsDeleted == false
                                          select new { Id = aircraftSchedule.Id }).Count() > 0;

                return isAlreadyCheckOut;
            }
        }

        public AircraftScheduleDetail CheckOut(AircraftScheduleDetail aircraftScheduleDetail)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftScheduleDetails.Add(aircraftScheduleDetail);
                _myContext.SaveChanges();

                return aircraftScheduleDetail;
            }
        }

        public AircraftScheduleDetail UnCheckOut(long id)
        {
            using (_myContext = new MyContext())
            {
                AircraftScheduleDetail aircraftScheduleDetail =  _myContext.AircraftScheduleDetails.Where(p=> p.Id == id).FirstOrDefault();
                
                if (aircraftScheduleDetail == null)
                    return null;

                _myContext.Remove(aircraftScheduleDetail);

                _myContext.SaveChanges();

                return aircraftScheduleDetail;
            }
        }

        public AircraftScheduleDetail CheckIn(long checkInBy, DateTime checkInTime, long aircraftScheduleId)
        {
            using (_myContext = new MyContext())
            {
                AircraftScheduleDetail aircraftScheduleDetail = _myContext.AircraftScheduleDetails.Where(p => p.AircraftScheduleId == aircraftScheduleId).FirstOrDefault();

                if (aircraftScheduleDetail != null)
                {
                    aircraftScheduleDetail.CheckInBy = checkInBy;
                    aircraftScheduleDetail.CheckInTime = checkInTime;
                    aircraftScheduleDetail.IsCheckOut = false;
                    aircraftScheduleDetail.FlightStatus = "CheckedIn";

                    _myContext.SaveChanges();
                }

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
