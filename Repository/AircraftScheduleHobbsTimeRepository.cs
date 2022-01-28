using DataModels.Entities;
using Repository.Interface;
using System.Collections.Generic;

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
    }
}
