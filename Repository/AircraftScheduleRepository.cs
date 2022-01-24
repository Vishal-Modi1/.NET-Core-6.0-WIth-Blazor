using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class AircraftScheduleRepository : IAircraftScheduleRepository
    {
        private MyContext _myContext;

        public AircraftSchedule Create(AircraftSchedule aircraftSchedule)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftSchedules.Add(aircraftSchedule);
                _myContext.SaveChanges();

                return aircraftSchedule;
            }
        }

        #region ActivityType
        public List<DropDownValues> ListActivityTypeDropDownValues(int roleId)
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> dropDownValues = new List<DropDownValues>();

                UserRoleVsScheduleActivityType userRoleVsScheduleActivityType = (from userRoleActivitiy in _myContext.UserRoleVsScheduleActivityType
                                                                                 where userRoleActivitiy.UserRoleId == roleId
                                                                                 select userRoleActivitiy).FirstOrDefault();

                if (userRoleVsScheduleActivityType != null)
                {
                    List<int> scheduleActivityIds = userRoleVsScheduleActivityType.ActivityTypeIds.Split(new char[] { ',' }).Select(p => Convert.ToInt32(p)).ToList();

                    dropDownValues = (from scheduleActivity in _myContext.ScheduleActivityTypes
                                     where scheduleActivityIds.Contains(scheduleActivity.Id)
                                     select new DropDownValues()
                                     {
                                         Id = scheduleActivity.Id,
                                         Name = scheduleActivity.Name,
                                     }).ToList();
                }

                return dropDownValues;
            }
        }

        #endregion
    }
}
