﻿using DataModels.Entities;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Service
{
    public class AircraftScheduleService : BaseService, IAircraftScheduleService
    {
        private readonly IAircraftScheduleRepository _aircraftScheduleRepository;
        private readonly IAircraftScheduleDetailRepository _aircraftScheduleDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAircraftEquipmentTimeRepository _aircraftEquipmentTimeRepository;
        private readonly IAircraftScheduleHobbsTimeRepository _aircraftScheduleHobbsTimeRepository;

        public AircraftScheduleService(IAircraftScheduleRepository aircraftScheduleRepository, IUserRepository userRepository,
            IAircraftRepository aircraftRepository, IAircraftScheduleDetailRepository aircraftScheduleDetailRepository,
            IAircraftEquipmentTimeRepository aircraftEquipmentTimeRepository,
            IAircraftScheduleHobbsTimeRepository aircraftScheduleHobbsTimeRepository)
        {
            _aircraftScheduleRepository = aircraftScheduleRepository;
            _userRepository = userRepository;
            _aircraftRepository = aircraftRepository;
            _aircraftScheduleDetailRepository = aircraftScheduleDetailRepository;
            _aircraftEquipmentTimeRepository = aircraftEquipmentTimeRepository;
            _aircraftScheduleHobbsTimeRepository = aircraftScheduleHobbsTimeRepository;
        }

        public CurrentResponse GetDetails(int roleId, int companyId, long id)
        {
            SchedulerVM schedulerVM = new SchedulerVM();

            try
            {
                AircraftSchedule aircraftSchedule = _aircraftScheduleRepository.FindByCondition(p => p.Id == id);

                if (aircraftSchedule != null)
                {
                    schedulerVM = ToBusinessObject(aircraftSchedule);
                    AircraftScheduleDetail aircraftScheduleDetail = _aircraftScheduleDetailRepository.FindByCondition(p => p.AircraftScheduleId == schedulerVM.Id);

                    SetAircraftScheduleDetails(schedulerVM, aircraftScheduleDetail);

                    schedulerVM.AircraftEquipmentsTimeList = _aircraftEquipmentTimeRepository.ListByCondition(p => p.AircraftId == schedulerVM.AircraftId && p.IsDeleted == false);
                    schedulerVM.AircraftEquipmentsTimeList.ForEach(p => { p.AircraftScheduleId = aircraftSchedule.Id; });
                  
                    SetAircraftEquipmentHobbsTime(schedulerVM);
                }

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

        private void SetAircraftEquipmentHobbsTime(SchedulerVM schedulerVM)
        {
            schedulerVM.AircraftEquipmentHobbsTimeList = new List<AircraftScheduleHobbsTime>();

            List<long> equipmentIdsList = schedulerVM.AircraftEquipmentsTimeList.Select(p => p.Id).ToList();

            schedulerVM.AircraftEquipmentHobbsTimeList = _aircraftScheduleHobbsTimeRepository.ListByCondition(p => equipmentIdsList.Contains(p.AircraftEquipmentTimeId) && p.AircraftScheduleId == schedulerVM.Id);
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

        public CurrentResponse EditEndTime(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM)
        {
            try
            {
                bool isAircraftAvailable =_aircraftScheduleRepository.IsAircraftAvailable(schedulerEndTimeDetailsVM);

                if(!isAircraftAvailable)
                {
                    CreateResponse(false, HttpStatusCode.OK, "Aircraft is not available for selected time duration");

                    return _currentResponse;
                }

                schedulerEndTimeDetailsVM.UpdatedOn = DateTime.UtcNow;

                _aircraftScheduleRepository.EditEndTime(schedulerEndTimeDetailsVM);

                CreateResponse(true, HttpStatusCode.OK, "Appointment updated successfully");

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

        private void SetAircraftScheduleDetails(SchedulerVM schedulerVM, AircraftScheduleDetail aircraftScheduleDetail)
        {
            schedulerVM.AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM();
            
            if (aircraftScheduleDetail != null)
            {
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = aircraftScheduleDetail.IsCheckOut;
                schedulerVM.AircraftSchedulerDetailsVM.CheckInTime = aircraftScheduleDetail.CheckInTime;
                schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime = aircraftScheduleDetail.CheckOutTime;
                schedulerVM.AircraftSchedulerDetailsVM.CheckInBy = aircraftScheduleDetail.CheckInBy;
                schedulerVM.AircraftSchedulerDetailsVM.CheckOutBy = aircraftScheduleDetail.CheckOutBy;
                schedulerVM.AircraftSchedulerDetailsVM.Id = aircraftScheduleDetail.Id;

                User checkInByUser = _userRepository.FindByCondition(p => p.Id == schedulerVM.AircraftSchedulerDetailsVM.CheckInBy);
                User checkOutByUser = _userRepository.FindByCondition(p => p.Id == schedulerVM.AircraftSchedulerDetailsVM.CheckOutBy);
         
                if(checkInByUser != null)
                {
                    schedulerVM.AircraftSchedulerDetailsVM.CheckInByUserName = $"{checkInByUser.FirstName} {checkInByUser.LastName}";
                }

                if (checkOutByUser != null)
                {
                    schedulerVM.AircraftSchedulerDetailsVM.CheckOutByUserName = $"{checkOutByUser.FirstName} {checkOutByUser.LastName}";
                }
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