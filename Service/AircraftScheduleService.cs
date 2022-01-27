using DataModels.Entities;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
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

        public CurrentResponse GetDetails(int roleId, int companyId, long id)
        {
            AircraftSchedule aircraftSchedule = _aircraftScheduleRepository.FindByCondition(p => p.Id == id);
            
            SchedulerVM schedulerVM = new SchedulerVM();

            if(aircraftSchedule != null)
            {
                schedulerVM = ToBusinessObject(aircraftSchedule);
            }

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
                CreateResponse(aircraftSchedule, HttpStatusCode.OK, "Aircraft Appointment created successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(SchedulerVM schedulerVM)
        {
            try
            {
                AircraftSchedule aircraftSchedule = ToDataObject(schedulerVM);
                aircraftSchedule = _aircraftScheduleRepository.Edit(aircraftSchedule);

                CreateResponse(aircraftSchedule, HttpStatusCode.OK, "Appointment updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(SchedulerFilter schedulerFilter)
        {
            try
            {
                List<SchedulerVM> schedulersList = _aircraftScheduleRepository.List(schedulerFilter);

                CreateResponse(schedulersList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(long id)
        {
            try
            {
                _aircraftScheduleRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Appointment deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse IsAircraftAlreadyCheckOut(long aircraftId)
        {
            try
            {
                bool response = _aircraftScheduleRepository.IsAircraftAlreadyCheckOut(aircraftId);

                if (response)
                {
                    CreateResponse(true, HttpStatusCode.OK, "Aircraft is already checked-out. The aircraft must be checked-in before you can check it out.");
                }
                else
                {
                    CreateResponse(false, HttpStatusCode.OK, "");
                }

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        #region Object Mapping
        private AircraftSchedule ToDataObject(SchedulerVM schedulerVM)
        {
            AircraftSchedule aircraftSchedule = new AircraftSchedule();

            Random rnd = new Random();

            aircraftSchedule.SchedulActivityTypeId = schedulerVM.ScheduleActivityId.GetValueOrDefault();
            aircraftSchedule.Id = schedulerVM.Id;

            if (schedulerVM.Id == 0)
            {
                aircraftSchedule.ReservationId = Guid.NewGuid();
            }
            
            aircraftSchedule.StartDateTime = schedulerVM.StartTime;
            aircraftSchedule.EndDateTime = schedulerVM.EndTime;
            aircraftSchedule.IsRecurring = schedulerVM.IsRecurring;
            aircraftSchedule.Member1Id = schedulerVM.Member1Id;
            aircraftSchedule.Member2Id = schedulerVM.Member2Id;
            aircraftSchedule.InstructorId = schedulerVM.InstructorId;
            aircraftSchedule.ScheduleTitle = schedulerVM.DisplayTitle;
            aircraftSchedule.AircraftId = schedulerVM.AircraftId;
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

        private SchedulerVM ToBusinessObject(AircraftSchedule aircraftSchedule)
        {
            SchedulerVM schedulerVM = new SchedulerVM();

            schedulerVM.Id = aircraftSchedule.Id;
            schedulerVM.ScheduleActivityId = aircraftSchedule.SchedulActivityTypeId;
            schedulerVM.ReservationId = aircraftSchedule.ReservationId;

            schedulerVM.StartTime = aircraftSchedule.StartDateTime;
            schedulerVM.EndTime = aircraftSchedule.EndDateTime;
            schedulerVM.IsRecurring = aircraftSchedule.IsRecurring;
            schedulerVM.Member1Id = aircraftSchedule.Member1Id;
            schedulerVM.Member2Id = aircraftSchedule.Member2Id;
            schedulerVM.InstructorId = aircraftSchedule.InstructorId;
            schedulerVM.DisplayTitle = aircraftSchedule.ScheduleTitle;
            schedulerVM.AircraftId = aircraftSchedule.AircraftId;
            schedulerVM.FlightType = aircraftSchedule.FlightType;
            schedulerVM.FlightRules = aircraftSchedule.FlightRules;
            schedulerVM.EstHours = aircraftSchedule.EstFlightHours;
            schedulerVM.Comments = aircraftSchedule.Comments;
            schedulerVM.InternalComments = aircraftSchedule.PrivateComments;
            schedulerVM.FlightRoutes = aircraftSchedule.FlightRoutes;
            schedulerVM.IsStandBy = aircraftSchedule.StandBy;

            return schedulerVM;
        }

        #endregion
    }
}
