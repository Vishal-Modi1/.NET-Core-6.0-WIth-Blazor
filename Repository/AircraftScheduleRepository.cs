using DataModels.Entities;
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
                                                     join aircraftScheduleDetail in _myContext.AircraftScheduleDetails 
                                                     on aircraftSchedules.Id equals aircraftScheduleDetail.AircraftScheduleId into asd
                                                     from details in asd.DefaultIfEmpty()
                                                     join user in _myContext.Users
                                                     on aircraftSchedules.Member1Id equals user.Id into u
                                                     from userDetails in u.DefaultIfEmpty()
                                                     where aircraft.CompanyId == schedulerFilter.CompanyId && aircraftSchedules.IsActive == true
                                                     && aircraftSchedules.StartDateTime.Date >= schedulerFilter.StartTime.Date
                                                     && aircraftSchedules.EndDateTime.Date <= schedulerFilter.EndTime.Date
                                                     && aircraftSchedules.IsDeleted == false
                                                     select new SchedulerVM()
                                                     {
                                                         Id = aircraftSchedules.Id,
                                                         DisplayTitle = aircraftSchedules.ScheduleTitle,
                                                         StartTime = Convert.ToDateTime(aircraftSchedules.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss")),
                                                         EndTime = aircraftSchedules.EndDateTime,
                                                         Comments = aircraftSchedules.Comments,
                                                         AircraftId = aircraftSchedules.AircraftId,
                                                         TailNo = aircraft.TailNo,
                                                         Member1 = userDetails == null ? "" : (userDetails.FirstName + " " + userDetails.LastName),
                                                         AircraftSchedulerDetailsVM = details == null ? 
                                                         new AircraftSchedulerDetailsVM() : 
                                                         new AircraftSchedulerDetailsVM()
                                                         {
                                                             IsCheckOut =  details.IsCheckOut,
                                                             CheckInTime = details.CheckInTime,
                                                             CheckOutTime = details.CheckOutTime,
                                                             CheckInBy = details.CheckInBy,
                                                             AircraftScheduleId = details.AircraftScheduleId,
                                                             Id = details.Id
                                                         }
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
                    existingAppointment.DepartureAirportId = aircraftSchedule.DepartureAirportId;
                    existingAppointment.ArrivalAirportId = aircraftSchedule.ArrivalAirportId;
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

        public void EditEndTime(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM)
        {
            using (_myContext = new MyContext())
            {
                AircraftSchedule existingAppointment = _myContext.AircraftSchedules.Where(p => p.Id == schedulerEndTimeDetailsVM.ScheduleId).FirstOrDefault();

                if (existingAppointment != null)
                {
                    existingAppointment.EndDateTime = schedulerEndTimeDetailsVM.EndTime;
                    existingAppointment.UpdatedOn = schedulerEndTimeDetailsVM.UpdatedOn;
                    existingAppointment.UpdatedBy = schedulerEndTimeDetailsVM.UpdatedBy;
                }

                _myContext.SaveChanges();
            }
        }

        public bool IsAircraftAvailable(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM)
        {
            using (_myContext = new MyContext())
            {
                bool isAircraftAvailable = _myContext.AircraftSchedules.Where(p=> p.Id != schedulerEndTimeDetailsVM.ScheduleId && 
                p.AircraftId == schedulerEndTimeDetailsVM.AircraftId && p.IsDeleted == false && p.IsActive == true
                && ((p.StartDateTime > schedulerEndTimeDetailsVM.StartTime && p.StartDateTime < schedulerEndTimeDetailsVM.EndTime)
                || (p.EndDateTime < schedulerEndTimeDetailsVM.EndTime && p.EndDateTime > schedulerEndTimeDetailsVM.StartTime)
                || (p.EndDateTime == schedulerEndTimeDetailsVM.EndTime && p.StartDateTime == schedulerEndTimeDetailsVM.StartTime))).Count() == 0;

                return isAircraftAvailable;
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

        public void Delete(long id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                AircraftSchedule aircraftSchedule = _myContext.AircraftSchedules.Where(p => p.Id == id).FirstOrDefault();

                if (aircraftSchedule != null)
                {
                    aircraftSchedule.IsDeleted = true;
                    aircraftSchedule.DeletedOn = DateTime.UtcNow;
                    aircraftSchedule.DeletedBy = deletedBy; 

                    _myContext.SaveChanges();
                }
            }
        }

      
        #region ActivityType
        public List<DropDownLargeValues> ListActivityTypeDropDownValues(int roleId)
        {
            using (_myContext = new MyContext())
            {
                List<DropDownLargeValues> dropDownValues = new List<DropDownLargeValues>();

                UserRoleVsScheduleActivityType userRoleVsScheduleActivityType = (from userRoleActivitiy in _myContext.UserRoleVsScheduleActivityType
                                                                                 where userRoleActivitiy.UserRoleId == roleId
                                                                                 select userRoleActivitiy).FirstOrDefault();

                if (userRoleVsScheduleActivityType != null)
                {
                    List<int> scheduleActivityIds = userRoleVsScheduleActivityType.ActivityTypeIds.Split(new char[] { ',' }).Select(p => Convert.ToInt32(p)).ToList();

                    dropDownValues = (from scheduleActivity in _myContext.ScheduleActivityTypes
                                      where scheduleActivityIds.Contains(scheduleActivity.Id)
                                      select new DropDownLargeValues()
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
