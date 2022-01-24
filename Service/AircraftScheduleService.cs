using DataModels.Entities;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

namespace Service
{
    public class AircraftScheduleService : BaseService, IAircraftScheduleService
    {
        private readonly IAircraftScheduleRepository _aircraftScheduleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAircraftRepository _aircraftRepository;

        public AircraftScheduleService(IAircraftScheduleRepository aircraftScheduleRepository, IUserRepository userRepository,
            IAircraftRepository aircraftRepository)
        {
            _aircraftScheduleRepository = aircraftScheduleRepository;
            _userRepository = userRepository;
            _aircraftRepository = aircraftRepository;
        }

        public CurrentResponse GetDetails(int roleId, int companyId)
        {
            SchedulerVM schedulerVM = new SchedulerVM();

            try
            {
                schedulerVM.ScheduleActivitiesList = _aircraftScheduleRepository.ListActivityTypeDropDownValues(roleId);
                schedulerVM.Member1List = schedulerVM.Member2List = _userRepository.ListDropdownValuesbyCondition(p => p.IsActive == true && p.IsDeleted == false && p.RoleId != (int)DataModels.Enums.UserRole.Instructors && p.CompanyId == companyId);
                schedulerVM.InstructorsList = _userRepository.ListDropdownValuesbyCondition(p => p.IsActive == true && p.IsDeleted == false && p.RoleId == (int)DataModels.Enums.UserRole.Instructors && p.CompanyId == companyId);
                schedulerVM.AircraftsList = _aircraftRepository.ListDropDownValues(companyId);

                CreateResponse(schedulerVM, HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                CreateResponse(schedulerVM, HttpStatusCode.InternalServerError, ex.Message);
            }

            return _currentResponse;
        }

        public CurrentResponse Create(SchedulerVM schedulerVM)
        {
            AircraftSchedule aircraftSchedule = ToDataObject(schedulerVM);

            try
            {
                aircraftSchedule = _aircraftScheduleRepository.Create(aircraftSchedule);
                CreateResponse(aircraftSchedule, HttpStatusCode.OK, "Aircraft scheduled successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private AircraftSchedule ToDataObject(SchedulerVM schedulerVM)
        {
            AircraftSchedule aircraftSchedule = new AircraftSchedule();

            Random rnd = new Random();

            aircraftSchedule.SchedulActivityTypeId = schedulerVM.ScheduleActivityId.GetValueOrDefault();

            if (schedulerVM.Id == 0)
            {
                aircraftSchedule.ReservationId = "#" + DateTime.Now.ToString("ddMMyyyyhhmmss") + rnd.Next();
            }
            
            aircraftSchedule.StartDateTime = schedulerVM.StartTime;
            aircraftSchedule.EndDateTime = schedulerVM.EndTime;
            aircraftSchedule.IsRecurring = schedulerVM.IsRecurring;
            aircraftSchedule.Member1Id = schedulerVM.Member1Id;
            aircraftSchedule.Member2Id = schedulerVM.Member2Id;
            aircraftSchedule.InstructorId = schedulerVM.InstructorId;
            aircraftSchedule.ScheduleTitle = schedulerVM.DisplayTitle;
            aircraftSchedule.AircraftId = schedulerVM.AircraftId.GetValueOrDefault();
            aircraftSchedule.FlightType = schedulerVM.FlightType;
            aircraftSchedule.FlightRules = schedulerVM.FlightRules;
            aircraftSchedule.EstFlightHours = schedulerVM.EstHours;
            aircraftSchedule.Comments = schedulerVM.Comments;
            aircraftSchedule.PrivateComments = schedulerVM.InternalComments;
            aircraftSchedule.FlightRoutes = schedulerVM.FlightRoutes;
            aircraftSchedule.StandBy = schedulerVM.IsStandBy;
            aircraftSchedule.IsActive = true;

            aircraftSchedule.CreatedBy = schedulerVM.CreatedBy;
            aircraftSchedule.UpdatedBy = schedulerVM.UpdatedBy;

            if (schedulerVM.Id == 0)
            {
                aircraftSchedule.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                aircraftSchedule.UpdatedOn = DateTime.UtcNow;
            }

            return aircraftSchedule;
        }
    }
}
