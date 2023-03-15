using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.LogBook;
using DataModels.Entities;
using DataModels.Constants;
using AutoMapper;

namespace Service
{
    public class LogBookService : BaseService, ILogBookService
    {
        private readonly ILogBookRepository _logBookRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LogBookService(ILogBookRepository logBookRepository, IUserRepository userRepository,
            IAircraftRepository aircraftRepository, ICompanyRepository companyRepository,
            IMapper mapper)
        {
            _logBookRepository = logBookRepository;
            _userRepository = userRepository;
            _aircraftRepository = aircraftRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        public CurrentResponse Create(LogBookVM logBookVM)
        {
            try
            {
                logBookVM.CreatedOn = DateTime.UtcNow;
                logBookVM.IsActive = true;
                logBookVM = _logBookRepository.Create(logBookVM);

                CreateResponse(logBookVM, HttpStatusCode.OK, "Logbook added successfully");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(LogBookVM logBookVM)
        {
            try
            {
                logBookVM.UpdatedOn = DateTime.UtcNow;
                logBookVM = _logBookRepository.Edit(logBookVM);

                CreateResponse(logBookVM, HttpStatusCode.OK, "Logbook updated successfully");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetFiltersValue(int companyId)
        {
            try
            {
                LogBookFilterVM logBookFilterVM = new LogBookFilterVM();

                if (companyId == 0)
                {
                    logBookFilterVM.Companies = _companyRepository.ListDropDownValues();
                }
                else
                {
                    logBookFilterVM.UsersList = _userRepository.ListDropdownValuesbyCompanyId(companyId);
                }

                CreateResponse(logBookFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new LogBookFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
        public CurrentResponse List(LogBookDatatableParams datatableParams)
        {
            try
            {
                List<LogBookDataVM> logbooksList = _logBookRepository.List(datatableParams);
                CreateResponse(logbooksList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetFiltersValue(string role, int companyId)
        {
            try
            {
                LogBookFilterVM logBookFilter = new LogBookFilterVM();

                if (role.Replace(" ", "") == DataModels.Enums.UserRole.SuperAdmin.ToString())
                {
                    logBookFilter.Companies = _companyRepository.ListDropDownValues();
                }
                else
                {
                    if (role.Replace(" ", "") == DataModels.Enums.UserRole.Admin.ToString())
                    {
                        logBookFilter.UsersList = _userRepository.ListDropdownValuesbyCompanyId(companyId);
                    }

                    logBookFilter.AircraftsList = _aircraftRepository.ListDropDownValuesByCompanyId(companyId);
                }

                CreateResponse(logBookFilter, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new LogBookFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindById(long id)
        {
            try
            {
                LogBookVM logBookVM = _logBookRepository.FindById(id);

                if (logBookVM != null)
                {
                    foreach (LogBookFlightPhotoVM item in logBookVM.LogBookFlightPhotosList)
                    {
                        item.ImagePath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.LogbookFlightPhoto}/{logBookVM.CompanyId}/{logBookVM.CreatedBy}/{id}/{item.Name}";
                    }
                }

                CreateResponse(logBookVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListInstrumentApproachesDropdownValues()
        {
            try
            {
                List<DropDownSmallValues> approachesLIst = _logBookRepository.ListInstrumentApproachesDropdownValues();

                CreateResponse(approachesLIst, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse LogBookSummaries(long userId, int companyId, string role)
        {
            try
            {
                List<LogBookSummaryVM> logBookSummaries = _logBookRepository.LogBookSummaries(userId, companyId, role);

                CreateResponse(logBookSummaries, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        #region flight photos

        public CurrentResponse UpdateImagesName(long logBookId, List<LogBookFlightPhoto> logBookFlightPhotosList)
        {
            try
            {
                _logBookRepository.UpdateImagesName(logBookId, logBookFlightPhotosList);

                CreateResponse(true, HttpStatusCode.OK, "Logbook added successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public List<LogBookFlightPhoto> ListFlightPhotosByLogBookId(long logBookId)
        {
            return _logBookRepository.ListFlightPhotosByLogBookId(logBookId);
        }

        public CurrentResponse DeletePhoto(long id, long deletedBy)
        {
            try
            {
                _logBookRepository.DeletePhoto(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Photo deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        #endregion

        #region Crew passengers

        public CurrentResponse ListPassengersRolesDropdownValues()
        {
            try
            {
                List<DropDownSmallValues> passengersRoles = _logBookRepository.ListPassengersRolesDropdownValues();
                CreateResponse(passengersRoles, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListPassengersDropdownValuesByCompanyId(int companyId)
        {
            try
            {
                List<DropDownLargeValues> passengersList = _logBookRepository.ListPassengersDropdownValuesByCompanyId(companyId);
                CreateResponse(passengersList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse CreateCrewPassenger(CrewPassengerVM crewPassengerVM)
        {
            try
            {
                CrewPassenger crewPassenger = _mapper.Map<CrewPassenger>(crewPassengerVM);
                crewPassenger = _logBookRepository.SaveCrewPassenger(crewPassenger);
                crewPassengerVM = _mapper.Map<CrewPassengerVM>(crewPassenger);

                CreateResponse(crewPassengerVM, HttpStatusCode.OK, "New member added successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse DeleteLogBookCrewPassenger(long id, long deletedBy)
        {
            try
            {
                _logBookRepository.DeleteLogBookCrewPassenger(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Passenger deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse DeleteLogBookInstrumentApproach(long id, long deletedBy)
        {
            try
            {
                _logBookRepository.DeleteLogBookInstrumentApproach(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Instrument approach deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _logBookRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "LogBook deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        #endregion
    }
}
