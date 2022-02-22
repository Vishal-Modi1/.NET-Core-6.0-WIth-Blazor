using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class AircraftScheduleHobbsTimeRepository : IAircraftScheduleHobbsTimeRepository
    {
        private MyContext _myContext;

        public List<AircraftScheduleHobbsTime> Create(List<AircraftScheduleHobbsTime> aircraftScheduleHobbsTimesList)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftScheduleHobbsTimes.AddRange(aircraftScheduleHobbsTimesList);
                _myContext.SaveChanges();

                return aircraftScheduleHobbsTimesList;
            }
        }

        public List<AircraftScheduleHobbsTime> Edit(List<AircraftScheduleHobbsTime> aircraftScheduleHobbsTimesList)
        {
            using (_myContext = new MyContext())
            {
                var idsList = aircraftScheduleHobbsTimesList.Select(p => p.Id).ToList();

                List<AircraftScheduleHobbsTime> existingDataList = _myContext.AircraftScheduleHobbsTimes.Where(p=> idsList.Contains(p.Id)).ToList();

                foreach (AircraftScheduleHobbsTime aircraftScheduleHobbsTime in existingDataList)
                {
                    AircraftScheduleHobbsTime updatedTimeData = aircraftScheduleHobbsTimesList.Where(p => p.Id == aircraftScheduleHobbsTime.Id).First();

                    aircraftScheduleHobbsTime.InTime = updatedTimeData.InTime;
                    aircraftScheduleHobbsTime.OutTime = updatedTimeData.OutTime;
                    aircraftScheduleHobbsTime.TotalTime = updatedTimeData.TotalTime;
                }

                _myContext.SaveChanges();

                return aircraftScheduleHobbsTimesList;
            }
        }

        public List<AircraftScheduleHobbsTime> ListByCondition(Expression<Func<AircraftScheduleHobbsTime, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftScheduleHobbsTimes.Where(predicate).ToList();
            }
        }
    }
}
