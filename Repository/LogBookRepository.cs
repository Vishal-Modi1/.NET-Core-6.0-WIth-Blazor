using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Repository
{
    public class LogBookRepository : BaseRepository<LogBook>, ILogBookRepository
    {
        private readonly MyContext _myContext;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public LogBookRepository(MyContext dbContext, IMapper mapper, IUserRepository userRepository)
            : base(dbContext)
        {
            this._myContext = dbContext;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public LogBookVM Create(LogBookVM logBookVM)
        {
            using var transaction = _myContext.Database.BeginTransaction();

            try
            {
                LogBook logBook = SaveLogBookDetails(logBookVM);
                LogBookTrainingDetail logBookTrainingDetail = SaveLogBookTrainingDetails(logBook.Id, logBookVM.LogBookTrainingDetailVM);
                LogBookFlightTimeDetail logBookFlightTimeDetail = SaveLogBookFlightTimeDetails(logBook.Id, logBookVM.LogBookFlightTimeDetailVM);
                LogBookInstrument logBookInstrument = SaveLogBookInstrumentDetails(logBook.Id, logBookVM.LogBookInstrumentVM);
                List<LogBookInstrumentApproach> logBookInstrumentApproachesList = SaveLogBookInstrumentApproachDetails(logBookInstrument.Id, logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList);
                List<LogBookCrewPassenger> logBookCrewPassengersList = SaveLogBookCrewPassengers(logBook.Id, logBookVM.LogBookCrewPassengersList);
                List<LogBookFlightPhoto> logBookFlightPhotosList = SaveLogBookFlightPhotos(logBook.Id, logBookVM.LogBookFlightPhotosList);

                logBookVM = _mapper.Map<LogBookVM>(logBook);
                logBookVM.LogBookTrainingDetailVM = _mapper.Map<LogBookTrainingDetailVM>(logBookTrainingDetail);
                logBookVM.LogBookFlightTimeDetailVM = _mapper.Map<LogBookFlightTimeDetailVM>(logBookFlightTimeDetail);
                logBookVM.LogBookInstrumentVM = _mapper.Map<LogBookInstrumentVM>(logBookInstrument);
                logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList = _mapper.Map<List<LogBookInstrumentApproachVM>>(logBookInstrumentApproachesList);
                logBookVM.LogBookCrewPassengersList = _mapper.Map<List<LogBookCrewPassengerVM>>(logBookCrewPassengersList);
                logBookVM.LogBookFlightPhotosList = _mapper.Map<List<LogBookFlightPhotoVM>>(logBookFlightPhotosList);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally
            {

            }

            return logBookVM;
        }

        public LogBookVM Edit(LogBookVM logBookVM)
        {
            using var transaction = _myContext.Database.BeginTransaction();

            try
            {
                LogBook logBook = UpdateLogBookDetails(logBookVM);
                LogBookTrainingDetail logBookTrainingDetail = UpdateLogBookTrainingDetails(logBook.Id, logBookVM.LogBookTrainingDetailVM);
                LogBookFlightTimeDetail logBookFlightTimeDetail = UpdateLogBookFlightTimeDetails(logBook.Id, logBookVM.LogBookFlightTimeDetailVM);
                LogBookInstrument logBookInstrument = UpdateLogBookInstrumentDetails(logBook.Id, logBookVM.LogBookInstrumentVM);
                List<LogBookInstrumentApproach> logBookInstrumentApproachesList = UpdateLogBookInstrumentApproachDetails(logBookInstrument.Id, logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList);
                List<LogBookCrewPassenger> logBookCrewPassengersList = UpdateLogBookCrewPassengers(logBook.Id, logBookVM.LogBookCrewPassengersList);
                List<LogBookFlightPhoto> logBookFlightPhotosList = UpdateLogBookFlightPhotos(logBook.Id, logBookVM.LogBookFlightPhotosList);

                logBookVM = _mapper.Map<LogBookVM>(logBook);
                logBookVM.LogBookTrainingDetailVM = _mapper.Map<LogBookTrainingDetailVM>(logBookTrainingDetail);
                logBookVM.LogBookFlightTimeDetailVM = _mapper.Map<LogBookFlightTimeDetailVM>(logBookFlightTimeDetail);
                logBookVM.LogBookInstrumentVM = _mapper.Map<LogBookInstrumentVM>(logBookInstrument);
                logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList = _mapper.Map<List<LogBookInstrumentApproachVM>>(logBookInstrumentApproachesList);
                logBookVM.LogBookCrewPassengersList = _mapper.Map<List<LogBookCrewPassengerVM>>(logBookCrewPassengersList);
                logBookVM.LogBookFlightPhotosList = _mapper.Map<List<LogBookFlightPhotoVM>>(logBookFlightPhotosList);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally
            {

            }

            return logBookVM;
        }
        public List<LogBookSummaryVM> LogBookSummaries(long userId, int companyId)
        {
            List<LogBookSummaryVM> logBookSummaries = (from logBook in _myContext.LogBooks
                                                       join aircraft in _myContext.Aircrafts
                                                       on logBook.AircraftId equals aircraft.Id
                                                       where logBook.CompanyId == companyId
                                                       && logBook.CreatedBy == userId
                                                       orderby logBook.CreatedOn descending
                                                       select new LogBookSummaryVM()
                                                       {
                                                           Id = logBook.Id,
                                                           Arrival = logBook.Arrival,
                                                           Departure = logBook.Departure,
                                                           Date = logBook.Date,
                                                           TailNo = aircraft.TailNo,
                                                       }).Take(5).ToList();

            return logBookSummaries;
        }

        public List<DropDownSmallValues> ListInstrumentApproachesDropdownValues()
        {
            List<DropDownSmallValues> approaches = (from approach in _myContext.InstrumentApproaches
                                                    select new DropDownSmallValues()
                                                    {
                                                        Id = approach.Id,
                                                        Name = approach.Name
                                                    }).ToList();

            return approaches;
        }

        public LogBookVM FindById(long id)
        {
            try
            {
                LogBook logBook = _myContext.LogBooks.FirstOrDefault(p => p.Id == id);

                if (logBook == null)
                    return null;

                LogBookVM logBookVM = _mapper.Map<LogBookVM>(logBook);

                LogBookTrainingDetail logBookTrainingDetail = _myContext.LogBookTrainingDetails.FirstOrDefault(p => p.LogBookId == id);

                if (logBookTrainingDetail != null)
                {
                    logBookVM.LogBookTrainingDetailVM = _mapper.Map<LogBookTrainingDetailVM>(logBookTrainingDetail);
                }

                LogBookInstrument logBookInstrument = _myContext.LogBookInstruments.FirstOrDefault(p => p.LogBookId == id);

                if (logBookInstrument != null)
                {
                    logBookVM.LogBookInstrumentVM = _mapper.Map<LogBookInstrumentVM>(logBookInstrument);

                    List<LogBookInstrumentApproach> logBookInstrumentApproachesList = _myContext.LogBookInstrumentApproaches.Where(p => p.LogBookInstrumentId == logBookInstrument.Id).ToList();
                    logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList = _mapper.Map<List<LogBookInstrumentApproachVM>>(logBookInstrumentApproachesList);
                }

                LogBookFlightTimeDetail logBookFlightTimeDetail = _myContext.LogBookFlightTimeDetails.FirstOrDefault(p => p.LogBookId == id);

                if (logBookFlightTimeDetail != null)
                {
                    logBookVM.LogBookFlightTimeDetailVM = _mapper.Map<LogBookFlightTimeDetailVM>(logBookFlightTimeDetail);
                }

                List<LogBookCrewPassenger> logBookCrewPassengersList = _myContext.LogBookCrewPassengers.Where(p => p.LogBookId == logBook.Id).ToList();

                if (logBookCrewPassengersList.Any())
                {
                    logBookVM.LogBookCrewPassengersList = _mapper.Map<List<LogBookCrewPassengerVM>>(logBookCrewPassengersList);
                }

                List<LogBookFlightPhoto> logBookFlightPhotosList = _myContext.LogBookFlightPhotos.Where(p => p.LogBookId == logBook.Id).ToList();

                if (logBookFlightPhotosList.Any())
                {
                    logBookVM.LogBookFlightPhotosList = _mapper.Map<List<LogBookFlightPhotoVM>>(logBookFlightPhotosList);
                }

                return logBookVM;
            }
            catch (Exception exc)
            {
                return new LogBookVM();
            }
        }

        #region flight photos

        public List<LogBookFlightPhoto> ListFlightPhotosByLogBookId(long logbookId)
        {
            List<LogBookFlightPhoto> existingLogBookFlightPhotosList = _myContext.LogBookFlightPhotos.Where(p => p.LogBookId == logbookId && !p.IsDeleted).ToList();

            return existingLogBookFlightPhotosList;
        }

        public void UpdateImagesName(long logbookId, List<LogBookFlightPhoto> logBookFlightPhotosList)
        {
            List<LogBookFlightPhoto> existingLogBookFlightPhotosList = _myContext.LogBookFlightPhotos.Where(p => p.LogBookId == logbookId && !p.IsDeleted).ToList();

            if (!existingLogBookFlightPhotosList.Any())
            {
                return;
            }

            foreach (LogBookFlightPhoto logBookFlightPhoto in existingLogBookFlightPhotosList)
            {
                logBookFlightPhoto.Name = existingLogBookFlightPhotosList.Where(p => p.Name == logBookFlightPhoto.Name).First().Name;
            }

            _myContext.SaveChanges();
        }

        public void DeletePhoto(long id, long deletedBy)
        {
            LogBookFlightPhoto logBookFlightPhoto = _myContext.LogBookFlightPhotos.Where(p => p.Id == id).FirstOrDefault();

            if (logBookFlightPhoto != null)
            {
                logBookFlightPhoto.IsDeleted = true;
                logBookFlightPhoto.DeletedBy = deletedBy;
                logBookFlightPhoto.DeletedOn = DateTime.UtcNow;

                _myContext.SaveChanges();
            }
        }

        #endregion

        #region Save Detais
        private LogBook SaveLogBookDetails(LogBookVM logBookVM)
        {
            //Logbook Details
            LogBook logBook = _mapper.Map<LogBook>(logBookVM);
            _myContext.LogBooks.Add(logBook);
            _myContext.SaveChanges();

            return logBook;
        }

        private LogBookTrainingDetail SaveLogBookTrainingDetails(long logbookId, LogBookTrainingDetailVM logBookTrainingDetailVM)
        {
            LogBookTrainingDetail logBookTrainingDetail = _mapper.Map<LogBookTrainingDetail>(logBookTrainingDetailVM);
            logBookTrainingDetail.LogBookId = logbookId;
            _myContext.LogBookTrainingDetails.Add(logBookTrainingDetail);
            _myContext.SaveChanges();

            return logBookTrainingDetail;
        }

        private LogBookInstrument SaveLogBookInstrumentDetails(long logbookId, LogBookInstrumentVM logBookInstrumentVM)
        {
            LogBookInstrument logBookInstrument = _mapper.Map<LogBookInstrument>(logBookInstrumentVM);
            logBookInstrument.LogBookId = logbookId;
            _myContext.LogBookInstruments.Add(logBookInstrument);
            _myContext.SaveChanges();

            return logBookInstrument;
        }

        private List<LogBookInstrumentApproach> SaveLogBookInstrumentApproachDetails(long logBookInstrumentId, List<LogBookInstrumentApproachVM> logBookInstrumentApproachesVMList)
        {
            if (logBookInstrumentApproachesVMList.Count() == 0)
            {
                return new();
            }

            List<LogBookInstrumentApproach> logBookInstrumentApproachesList = _mapper.Map<List<LogBookInstrumentApproach>>(logBookInstrumentApproachesVMList);

            logBookInstrumentApproachesList.ForEach(p => { p.LogBookInstrumentId = logBookInstrumentId; });
            _myContext.LogBookInstrumentApproaches.AddRange(logBookInstrumentApproachesList);
            _myContext.SaveChanges();

            return logBookInstrumentApproachesList;
        }

        private LogBookFlightTimeDetail SaveLogBookFlightTimeDetails(long logBookId, LogBookFlightTimeDetailVM logBookFlightTimeDetailVM)
        {
            LogBookFlightTimeDetail logBookFlightTimeDetail = _mapper.Map<LogBookFlightTimeDetail>(logBookFlightTimeDetailVM);
            logBookFlightTimeDetail.LogBookId = logBookId;
            _myContext.LogBookFlightTimeDetails.Add(logBookFlightTimeDetail);
            _myContext.SaveChanges();

            return logBookFlightTimeDetail;
        }

        private List<LogBookCrewPassenger> SaveLogBookCrewPassengers(long logBookId, List<LogBookCrewPassengerVM> logBookCrewPassengers)
        {
            if (logBookCrewPassengers.Count() == 0)
            {
                return new();
            }

            List<LogBookCrewPassenger> logBookCrewPassengersList = _mapper.Map<List<LogBookCrewPassenger>>(logBookCrewPassengers);

            logBookCrewPassengersList.ForEach(p => { p.LogBookId = logBookId; });
            _myContext.LogBookCrewPassengers.AddRange(logBookCrewPassengersList);
            _myContext.SaveChanges();

            return logBookCrewPassengersList;
        }

        private List<LogBookFlightPhoto> SaveLogBookFlightPhotos(long logBookId, List<LogBookFlightPhotoVM> logBookFlightPhotos)
        {
            if (logBookFlightPhotos.Count() == 0)
            {
                return new();
            }

            List<LogBookFlightPhoto> logBookFlightPhotosList = _mapper.Map<List<LogBookFlightPhoto>>(logBookFlightPhotos);

            logBookFlightPhotosList.ForEach(p => { p.LogBookId = logBookId; });
            _myContext.LogBookFlightPhotos.AddRange(logBookFlightPhotosList);
            _myContext.SaveChanges();

            return logBookFlightPhotosList;
        }
        #endregion

        #region Update Detais

        private LogBook UpdateLogBookDetails(LogBookVM logBookVM)
        {
            LogBook logBook = _myContext.LogBooks.Where(p => p.Id == logBookVM.Id).FirstOrDefault();

            if (logBook == null)
            {
                return logBook;
            }

            _mapper.Map(logBookVM, logBook);
            _myContext.SaveChanges();

            return logBook;
        }

        private LogBookTrainingDetail UpdateLogBookTrainingDetails(long logbookId, LogBookTrainingDetailVM logBookTrainingDetailVM)
        {
            LogBookTrainingDetail logBookTrainingDetail = _myContext.LogBookTrainingDetails.Where(p => p.LogBookId == logbookId).FirstOrDefault();
           
            if (logBookTrainingDetail == null)
            {
                return logBookTrainingDetail;
            }

            _mapper.Map(logBookTrainingDetailVM, logBookTrainingDetail);
            _myContext.SaveChanges();

            return logBookTrainingDetail;
        }

        private LogBookInstrument UpdateLogBookInstrumentDetails(long logBookId, LogBookInstrumentVM logBookInstrumentVM)
        {
            LogBookInstrument logBookInstrument = _myContext.LogBookInstruments.Where(p => p.LogBookId == logBookId).FirstOrDefault();

            if (logBookInstrument == null)
            {
                return logBookInstrument;
            }

            _mapper.Map(logBookInstrumentVM, logBookInstrument);
            _myContext.SaveChanges();

            return logBookInstrument;
        }

        private List<LogBookInstrumentApproach> UpdateLogBookInstrumentApproachDetails(long logBookInstrumentId, List<LogBookInstrumentApproachVM> logBookInstrumentApproachesVMList)
        {
            if (logBookInstrumentApproachesVMList.Count() == 0)
            {
                return new();
            }

            List<LogBookInstrumentApproach> logBookInstrumentApproachesList = _myContext.LogBookInstrumentApproaches.Where(p => p.LogBookInstrumentId == logBookInstrumentId).ToList();

            _mapper.Map(logBookInstrumentApproachesVMList, logBookInstrumentApproachesList);
            _myContext.SaveChanges();

            return logBookInstrumentApproachesList;
        }

        private LogBookFlightTimeDetail UpdateLogBookFlightTimeDetails(long logBookId, LogBookFlightTimeDetailVM logBookFlightTimeDetailVM)
        {
            LogBookFlightTimeDetail logBookFlightTimeDetail = _myContext.LogBookFlightTimeDetails.Where(p => p.LogBookId == logBookId).FirstOrDefault();

            if (logBookFlightTimeDetail == null)
            {
                return logBookFlightTimeDetail;
            }

            _mapper.Map(logBookFlightTimeDetailVM, logBookFlightTimeDetail);
            _myContext.SaveChanges();

            return logBookFlightTimeDetail;
        }

        private List<LogBookCrewPassenger> UpdateLogBookCrewPassengers(long logBookId, List<LogBookCrewPassengerVM> logBookCrewPassengers)
        {
            if (logBookCrewPassengers.Count() == 0)
            {
                return new();
            }

            List<LogBookCrewPassenger> logBookCrewPassengersList = _myContext.LogBookCrewPassengers.Where(p => p.LogBookId == logBookId).ToList();

            _mapper.Map(logBookCrewPassengers, logBookCrewPassengersList);

            var newList = logBookCrewPassengers.Where(p => p.Id == 0).ToList();
            logBookCrewPassengersList.RemoveAll(p => p.Id == 0);

            if (newList.Any())
            {
                logBookCrewPassengersList.AddRange(SaveLogBookCrewPassengers(logBookId, newList));
            }

            _myContext.SaveChanges();

            return logBookCrewPassengersList;
        }

        private List<LogBookFlightPhoto> UpdateLogBookFlightPhotos(long logBookId, List<LogBookFlightPhotoVM> logBookFlightPhotos)
        {
            if (logBookFlightPhotos.Count() == 0)
            {
                return new();
            }

            List<LogBookFlightPhoto> logBookFlightPhotosList = _myContext.LogBookFlightPhotos.Where(p => p.LogBookId == logBookId).ToList();

            _mapper.Map(logBookFlightPhotos, logBookFlightPhotosList);
            _myContext.SaveChanges();

            return logBookFlightPhotosList;
        }
        #endregion

        #region Crew Passengers

        public List<DropDownSmallValues> ListPassengersRolesDropdownValues()
        {
            List<DropDownSmallValues> passengerRoles = _myContext.CrewPassengersRoles.Select(p => new DropDownSmallValues() { 
            Id = p.Id,
            Name = p.Name
            }).ToList();

            return passengerRoles;
        }

        public List<DropDownLargeValues> ListPassengersDropdownValuesByCompanyId(int companyId)
        {
            List<CrewPassenger> crewPassengersList = _myContext.CrewPassengers.Where(p => p.CompanyId == companyId).ToList();
            List<DropDownLargeValues> passengersList = crewPassengersList.Select(p => new DropDownLargeValues() { Id = p.Id, Name = p.Name}).ToList() ;

            return passengersList;
        }

        public CrewPassenger SaveCrewPassenger(CrewPassenger crewPassenger)
        {
            _myContext.CrewPassengers.Add(crewPassenger);
            _myContext.SaveChanges();

            return crewPassenger;
        }

        public void DeleteLogBookCrewPassenger(long id, long deletedBy)
        {
            LogBookCrewPassenger logBookCrewPassenger = _myContext.LogBookCrewPassengers.Where(p => p.Id == id).FirstOrDefault();

            if (logBookCrewPassenger != null)
            {
                logBookCrewPassenger.IsDeleted = true;
                logBookCrewPassenger.DeletedBy = deletedBy;
                logBookCrewPassenger.DeletedOn = DateTime.UtcNow;

                _myContext.SaveChanges();
            }
        }

        #endregion

    }
}


