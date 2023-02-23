using DataModels.Entities;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Utilities;

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
        private readonly IUserPreferenceRepository _userPreferenceRepository;
        private readonly IUserVSCompanyRepository _userVSCompanyRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISendMailService _sendMailService;
        private readonly IEmailConfigurationRepository _emailConfigurationRepository;
        private readonly IFlightCategoryRepository _flightCategoryRepository;
        private readonly IFlightTagRepository _flightTagRepository;

        public AircraftScheduleService(IAircraftScheduleRepository aircraftScheduleRepository, IUserRepository userRepository,
            IAircraftRepository aircraftRepository, IAircraftScheduleDetailRepository aircraftScheduleDetailRepository,
            IAircraftEquipmentTimeRepository aircraftEquipmentTimeRepository, ICompanyRepository companyRepository,
            IAircraftScheduleHobbsTimeRepository aircraftScheduleHobbsTimeRepository,
            IUserPreferenceRepository userPreferenceRepository, IUserVSCompanyRepository userVSCompanyRepository,
            IEmailConfigurationRepository emailConfigurationRepository, IFlightCategoryRepository flightCategoryRepository,
            IFlightTagRepository flightTagRepository)
        {
            _aircraftScheduleRepository = aircraftScheduleRepository;
            _userRepository = userRepository;
            _aircraftRepository = aircraftRepository;
            _aircraftScheduleDetailRepository = aircraftScheduleDetailRepository;
            _aircraftEquipmentTimeRepository = aircraftEquipmentTimeRepository;
            _aircraftScheduleHobbsTimeRepository = aircraftScheduleHobbsTimeRepository;
            _userPreferenceRepository = userPreferenceRepository;
            _userVSCompanyRepository = userVSCompanyRepository;
            _companyRepository = companyRepository;
            _emailConfigurationRepository = emailConfigurationRepository;
            _flightCategoryRepository = flightCategoryRepository;
            _sendMailService = new SendMailService();
            _flightTagRepository = flightTagRepository;
        }

        public CurrentResponse GetDetails(int roleId, int companyId, long id, long userId)
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

                    if (schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut)
                    {
                        schedulerVM.AircraftSchedulerDetailsVM.FlightStatus = "CheckedOut";
                    }
                    else
                    {
                        if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime != null)
                        {
                            schedulerVM.AircraftSchedulerDetailsVM.FlightStatus = "CheckedIn";
                        }
                        else
                        {
                            schedulerVM.AircraftSchedulerDetailsVM.FlightStatus = "Scheduled";
                        }
                    }
                }

                if (roleId == (int)DataModels.Enums.UserRole.SuperAdmin)
                {
                    schedulerVM.CompaniesList = _companyRepository.ListDropDownValues();

                    if (schedulerVM.AircraftId != null)
                    {
                        schedulerVM.CompanyId = companyId = _aircraftRepository.FindByCondition(p => p.Id == schedulerVM.AircraftId).CompanyId.GetValueOrDefault();
                    }
                }
                else
                {
                    schedulerVM.CompanyId = companyId;
                }

                if (companyId != 0)
                {
                    schedulerVM.UsersList = _userRepository.ListDropdownValuesbyCompanyId(companyId);
                }

                ListDropDownValues(schedulerVM, companyId, roleId);
                FilterValuesByUserPreferences(schedulerVM, userId);

                schedulerVM.RoleId = roleId;

                CreateResponse(schedulerVM, HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.Message);
            }

            return _currentResponse;
        }

        public CurrentResponse GetDropdownValuesByCompanyId(int roleId, int companyId, long userId)
        {
            SchedulerVM schedulerVM = new SchedulerVM();

            try
            {
                ListDropDownValues(schedulerVM, companyId, roleId);
                CreateResponse(schedulerVM, HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.Message);
            }

            return _currentResponse;
        }

        private void FilterValuesByUserPreferences(SchedulerVM schedulerVM, long userId)
        {
            List<UserPreference> userPreferencesList = _userPreferenceRepository.ListByUserId(userId);

            foreach (UserPreference userPreference in userPreferencesList)
            {
                if (userPreference.PreferenceType == PreferenceType.Aircraft.ToString())
                {
                    UserPreference aircraftData = userPreferencesList.Where(p => p.PreferenceType == PreferenceType.Aircraft.ToString()).FirstOrDefault();

                    if (aircraftData == null || string.IsNullOrWhiteSpace(aircraftData.PreferencesIds))
                    {
                        continue;
                    }

                    List<long> aircraftIds = aircraftData.PreferencesIds.Split(new char[] { ',' }).Select(p => Convert.ToInt64(p)).ToList();
                    schedulerVM.AircraftsList = schedulerVM.AircraftsList.Where(p => aircraftIds.Contains(p.Id)).ToList();
                }
                else if (userPreference.PreferenceType == PreferenceType.ScheduleActivityType.ToString())
                {
                    UserPreference activityType = userPreferencesList.Where(p => p.PreferenceType == PreferenceType.ScheduleActivityType.ToString()).FirstOrDefault();

                    if (activityType == null || string.IsNullOrWhiteSpace(activityType.PreferencesIds))
                    {
                        continue;
                    }

                    List<long> activityTypeIds = activityType.PreferencesIds.Split(new char[] { ',' }).Select(p => Convert.ToInt64(p)).ToList();
                    schedulerVM.ScheduleActivitiesList = schedulerVM.ScheduleActivitiesList.Where(p => activityTypeIds.Contains(p.Id)).ToList();
                }
            }
        }

        private void ListDropDownValues(SchedulerVM schedulerVM, int companyId, int roleId)
        {
            schedulerVM.ScheduleActivitiesList = _aircraftScheduleRepository.ListActivityTypeDropDownValues(roleId);

            var usersList = _userRepository.ListDropdownValuesbyCompanyId(companyId);
            List<UserVSCompany> userVsRoleList = _userVSCompanyRepository.ListByCompanyId(companyId);

            List<long> instructorsList = userVsRoleList.Where(p => p.RoleId == (int)DataModels.Enums.UserRole.Instructors).Select(p => p.UserId).ToList();
            List<long> pilotsList = userVsRoleList.Where(p => p.RoleId == (int)DataModels.Enums.UserRole.PilotRenter).Select(p => p.UserId).ToList();

            schedulerVM.Member1List = usersList.Where(p => pilotsList.Contains(p.Id)).ToList();
            schedulerVM.Member2List = usersList.Where(p => !instructorsList.Contains(p.Id)).ToList();
            schedulerVM.InstructorsList = usersList.Where(p => instructorsList.Contains(p.Id)).ToList();

            schedulerVM.AircraftsList = _aircraftRepository.ListDropDownValuesByCompanyId(companyId);
            schedulerVM.FlightCategoriesList = _flightCategoryRepository.ListDropDownValuesByCompanyId(companyId);
            schedulerVM.FlightTagsList = _flightTagRepository.ListDropDownValues(companyId);
        }

        public CurrentResponse ListActivityTypeDropDownValues(int roleId)
        {
            try
            {
                List<DropDownLargeValues> scheduleActivitiesList = _aircraftScheduleRepository.ListActivityTypeDropDownValues(roleId);
                CreateResponse(scheduleActivitiesList, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private void SetAircraftEquipmentHobbsTime(SchedulerVM schedulerVM)
        {
            schedulerVM.AircraftEquipmentHobbsTimeList = new List<AircraftScheduleHobbsTime>();

            List<long> equipmentIdsList = schedulerVM.AircraftEquipmentsTimeList.Select(p => p.Id).ToList();

            schedulerVM.AircraftEquipmentHobbsTimeList = _aircraftScheduleHobbsTimeRepository.ListByCondition(p => equipmentIdsList.Contains(p.AircraftEquipmentTimeId) && p.AircraftScheduleId == schedulerVM.Id);
        }

        public CurrentResponse Create(SchedulerVM schedulerVM, string timezone)
        {
            try
            {
                bool isAircraftAvailable = IsAircraftAvailable(schedulerVM);

                if (!isAircraftAvailable)
                {
                    CreateResponse(null, HttpStatusCode.NotAcceptable, "Aircraft is not available for selected time duration");

                    return _currentResponse;
                }

                AircraftSchedule aircraftSchedule = ToDataObject(schedulerVM);
                aircraftSchedule = _aircraftScheduleRepository.Create(aircraftSchedule);
                SendScheduleMail(schedulerVM, "You appointment has been scheduled.", timezone);
                CreateResponse(aircraftSchedule, HttpStatusCode.OK, "Appointment created successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(SchedulerVM schedulerVM, string timezone)
        {
            try
            {
                bool isAircraftAvailable = IsAircraftAvailable(schedulerVM);

                if (!isAircraftAvailable)
                {
                    CreateResponse(null, HttpStatusCode.NotAcceptable, "Aircraft is not available for selected time duration");

                    return _currentResponse;
                }

                AircraftSchedule existingAircraftSchedule = _aircraftScheduleRepository.FindByCondition(p => p.Id == schedulerVM.Id);
                AircraftSchedule aircraftSchedule = ToDataObject(schedulerVM);
                aircraftSchedule = _aircraftScheduleRepository.Edit(aircraftSchedule);

                bool isScheduleChanged = IsScheduleChanged(existingAircraftSchedule, schedulerVM);

                if (isScheduleChanged)
                {
                    SendScheduleMail(schedulerVM, "You appointment has been updated", timezone);
                }

                CreateResponse(aircraftSchedule, HttpStatusCode.OK, "Appointment updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private bool IsScheduleChanged(AircraftSchedule existingAircraftSchedule, SchedulerVM schedulerVM)
        {
            if (existingAircraftSchedule.InstructorId != schedulerVM.InstructorId || existingAircraftSchedule.Member1Id != schedulerVM.Member1Id
                || existingAircraftSchedule.Member2Id != schedulerVM.Member2Id || existingAircraftSchedule.StartDateTime != schedulerVM.StartTime
                || existingAircraftSchedule.EndDateTime != schedulerVM.EndTime || existingAircraftSchedule.AircraftId != schedulerVM.AircraftId
                || existingAircraftSchedule.DepartureAirportName != schedulerVM.DepartureAirport || existingAircraftSchedule.ArrivalAirportName
                != schedulerVM.ArrivalAirport || existingAircraftSchedule.ScheduleTitle != schedulerVM.DisplayTitle)
            {
                return true;
            }

            return false;
        }

        private bool IsAircraftAvailable(SchedulerVM schedulerVM)
        {
            SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM = new SchedulerEndTimeDetailsVM();
            schedulerEndTimeDetailsVM.ScheduleId = schedulerVM.Id;
            schedulerEndTimeDetailsVM.StartTime = schedulerVM.StartTime;
            schedulerEndTimeDetailsVM.EndTime = schedulerVM.EndTime;
            schedulerEndTimeDetailsVM.AircraftId = schedulerVM.AircraftId.GetValueOrDefault();

            bool isAircraftAvailable = _aircraftScheduleRepository.IsAircraftAvailable(schedulerEndTimeDetailsVM);

            return isAircraftAvailable;
        }

        public CurrentResponse EditEndTime(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM)
        {
            try
            {
                bool isAircraftAvailable = _aircraftScheduleRepository.IsAircraftAvailable(schedulerEndTimeDetailsVM);

                if (!isAircraftAvailable)
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

                foreach (var schedulerVM in schedulersList)
                {
                    if (schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut)
                    {
                        schedulerVM.AircraftSchedulerDetailsVM.FlightStatus = "CheckedOut";
                    }
                    else
                    {
                        if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime != null)
                        {
                            schedulerVM.AircraftSchedulerDetailsVM.FlightStatus = "CheckedIn";
                        }
                        else
                        {
                            schedulerVM.AircraftSchedulerDetailsVM.FlightStatus = "Scheduled";
                        }
                    }
                }

                CreateResponse(schedulersList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _aircraftScheduleRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Appointment deleted successfully");

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

                if (checkInByUser != null)
                {
                    schedulerVM.AircraftSchedulerDetailsVM.CheckInByUserName = $"{checkInByUser.FirstName} {checkInByUser.LastName}";
                }

                if (checkOutByUser != null)
                {
                    schedulerVM.AircraftSchedulerDetailsVM.CheckOutByUserName = $"{checkOutByUser.FirstName} {checkOutByUser.LastName}";
                }
            }
        }

        private void SendScheduleMail(SchedulerVM schedulerVM, string message, string timezone)
        {
            try
            {
                AppointmentCreatedSendEmailViewModel viewModel = new();

                viewModel.StartTime = DateConverter.ToLocal(schedulerVM.StartTime, timezone);
                viewModel.EndTime = DateConverter.ToLocal(schedulerVM.EndTime, timezone);
                viewModel.DepartureAirport = schedulerVM.DepartureAirport;
                viewModel.ArrivalAirport = schedulerVM.ArrivalAirport;

                byte[] encodedBytes = Encoding.UTF8.GetBytes(schedulerVM.Id.ToString() + "FlyManager");
                var data = Encoding.Default.GetBytes(schedulerVM.Id.ToString());
                viewModel.Link = $"{Configuration.ConfigurationSettings.Instance.WebURL}scheduler?ScheduleId={Convert.ToBase64String(encodedBytes)}";

                User user = new User();

                if (schedulerVM.Member2Id != null)
                {
                    user = _userRepository.FindByCondition(p => p.Id == schedulerVM.Member2Id);
                    viewModel.Member2 = user.FirstName + " " + user.LastName;
                }
                else if (schedulerVM.InstructorId != null)
                {
                    user = _userRepository.FindByCondition(p => p.Id == schedulerVM.InstructorId);
                    viewModel.Member2 = user.FirstName + " " + user.LastName;
                }

                viewModel.Aircraft = schedulerVM.AircraftsList.Where(p => p.Id == schedulerVM.AircraftId).First().Name;
                viewModel.ActivityType = schedulerVM.ScheduleActivitiesList.Where(p => p.Id == schedulerVM.ScheduleActivityId).First().Name;

                // Sending mail to first member
                viewModel.Message = message;

                user = _userRepository.FindByCondition(p => p.Id == schedulerVM.Member1Id);
                viewModel.ToEmail = user.Email;
                viewModel.Member1 = viewModel.UserName = user.FirstName + " " + user.LastName;

                viewModel.CompanyEmail = GetCompanyEmail(schedulerVM.CompanyId);
                _sendMailService.AppointmentCreated(viewModel);

                // Sending mail to co-member

                if (schedulerVM.Member2Id != null)
                {
                    viewModel.Message = message;
                    user = _userRepository.FindByCondition(p => p.Id == schedulerVM.Member2Id);
                    viewModel.ToEmail = user.Email;
                    viewModel.Member2 = viewModel.UserName = user.FirstName + " " + user.LastName;

                    viewModel.CompanyEmail = GetCompanyEmail(schedulerVM.CompanyId);
                    _sendMailService.AppointmentCreated(viewModel);
                }
                else if (schedulerVM.InstructorId != null)
                {
                    viewModel.Message = $"{viewModel.Member1} has scheduled an appointment. you are the part of an appointment.";
                    user = _userRepository.FindByCondition(p => p.Id == schedulerVM.InstructorId);
                    viewModel.ToEmail = user.Email;
                    viewModel.Member2 = viewModel.UserName = user.FirstName + " " + user.LastName;

                    viewModel.CompanyEmail = GetCompanyEmail(schedulerVM.CompanyId);
                    _sendMailService.AppointmentCreated(viewModel);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string GetCompanyEmail(int companyId)
        {
            EmailConfiguration emailConfiguration = _emailConfigurationRepository.FindByCondition(p => p.EmailTypeId == (byte)EmailTypes.Reservation && p.CompanyId == companyId);

            if (emailConfiguration != null && !string.IsNullOrWhiteSpace(emailConfiguration.Email))
            {
                return emailConfiguration.Email;
            }

            return "";
        }

        #region Object Mapping
        private AircraftSchedule ToDataObject(SchedulerVM schedulerVM)
        {
            AircraftSchedule aircraftSchedule = new AircraftSchedule();

            aircraftSchedule.SchedulActivityTypeId = Convert.ToInt32(schedulerVM.ScheduleActivityId.GetValueOrDefault());
            aircraftSchedule.Id = schedulerVM.Id;

            if (schedulerVM.Id == 0)
            {
                aircraftSchedule.ReservationId = Guid.NewGuid();
            }

            aircraftSchedule.CompanyId = schedulerVM.CompanyId;
            aircraftSchedule.StartDateTime = schedulerVM.StartTime;
            aircraftSchedule.EndDateTime = schedulerVM.EndTime;
            aircraftSchedule.DepartureAirportId = schedulerVM.DepartureAirportId;
            aircraftSchedule.DepartureAirportName = schedulerVM.DepartureAirport;
            aircraftSchedule.ArrivalAirportId = schedulerVM.ArrivalAirportId;
            aircraftSchedule.ArrivalAirportName = schedulerVM.ArrivalAirport;
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
            aircraftSchedule.FlightCategoryId = schedulerVM.FlightCategoryId;
            aircraftSchedule.FlightTagIds = schedulerVM.FlightTagIds;
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

            schedulerVM.CompanyId = aircraftSchedule.CompanyId;
            schedulerVM.StartTime = aircraftSchedule.StartDateTime;
            schedulerVM.EndTime = aircraftSchedule.EndDateTime;
            schedulerVM.DepartureAirportId = aircraftSchedule.DepartureAirportId;
            schedulerVM.DepartureAirport = aircraftSchedule.DepartureAirportName;
            schedulerVM.ArrivalAirportId = aircraftSchedule.ArrivalAirportId;
            schedulerVM.ArrivalAirport = aircraftSchedule.ArrivalAirportName;
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
            schedulerVM.FlightCategoryId = aircraftSchedule.FlightCategoryId;
            schedulerVM.FlightTagIds = aircraftSchedule.FlightTagIds;
            schedulerVM.IsStandBy = aircraftSchedule.StandBy;
            schedulerVM.CreatedBy = aircraftSchedule.CreatedBy;

            return schedulerVM;
        }

        #endregion
    }
}
