﻿using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class AircraftScheduleRepository : IAircraftScheduleRepository
    {
        private MyContext _myContext;

        public List<SchedulerVM> List(SchedulerFilter schedulerFilter)
        {
            using (_myContext = new MyContext())
            {
                List<SchedulerVM> companyDataList = (from aircraft in _myContext.AirCrafts
                                                     join aircraftSchedules in _myContext.AircraftSchedules
                                                     on aircraft.Id equals aircraftSchedules.AircraftId
                                                     where aircraft.CompanyId == schedulerFilter.CompanyId && aircraftSchedules.IsActive == true
                                                     && aircraftSchedules.IsDeleted == false
                                                     select new SchedulerVM()
                                                     {
                                                         Id = aircraftSchedules.Id,
                                                         DisplayTitle = aircraftSchedules.ScheduleTitle,
                                                         StartTime = aircraftSchedules.StartDateTime,
                                                         EndTime = aircraftSchedules.EndDateTime,
                                                         Comments = aircraftSchedules.Comments,
                                                         AircraftId = aircraftSchedules.AircraftId

                                                     }).ToList();

                return companyDataList;

            }
        }

        public AircraftSchedule Edit(AircraftSchedule aircraftSchedule)
        {
            using (_myContext = new MyContext())
            {
                AircraftSchedule existingAppointment = _myContext.AircraftSchedules.Where(p => p.Id == aircraftSchedule.Id).FirstOrDefault();

                if (existingAppointment != null)
                {
                    existingAppointment.AircraftId = aircraftSchedule.AircraftId; 
                    existingAppointment.SchedulActivityTypeId = aircraftSchedule.SchedulActivityTypeId; 
                    existingAppointment.StartDateTime = aircraftSchedule.StartDateTime; 
                    existingAppointment.EndDateTime = aircraftSchedule.EndDateTime; 
                    existingAppointment.IsRecurring = aircraftSchedule.IsRecurring; 
                    existingAppointment.Member1Id = aircraftSchedule.Member1Id; 
                    existingAppointment.Member2Id = aircraftSchedule.Member2Id; 
                    existingAppointment.InstructorId = aircraftSchedule.InstructorId; 
                    existingAppointment.ScheduleTitle = aircraftSchedule.ScheduleTitle; 
                    existingAppointment.FlightType = aircraftSchedule.FlightType; 
                    existingAppointment.FlightRules = aircraftSchedule.FlightRules; 
                    existingAppointment.Comments = aircraftSchedule.Comments; 
                    existingAppointment.PrivateComments = aircraftSchedule.PrivateComments; 
                    existingAppointment.FlightRoutes = aircraftSchedule.FlightRoutes; 
                    existingAppointment.StandBy = aircraftSchedule.StandBy; 
                    
                    existingAppointment.UpdatedOn = aircraftSchedule.UpdatedOn; 
                    existingAppointment.UpdatedBy = aircraftSchedule.UpdatedBy;
                }

                _myContext.SaveChanges();

                return aircraftSchedule;
            }
        }
        public AircraftSchedule Create(AircraftSchedule aircraftSchedule)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftSchedules.Add(aircraftSchedule);
                _myContext.SaveChanges();

                return aircraftSchedule;
            }
        }

        public AircraftSchedule FindByCondition(Expression<Func<AircraftSchedule, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftSchedules.Where(predicate).FirstOrDefault();
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
