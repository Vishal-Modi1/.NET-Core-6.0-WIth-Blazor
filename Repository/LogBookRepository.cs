using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public LogBookRepository(MyContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            this._myContext = dbContext;
            _mapper = mapper;
        }

        public void Create(LogBookVM logBookVM)
        {
            using var transaction = _myContext.Database.BeginTransaction();

            try
            {
                //Logbook Details
                LogBook logBook = _mapper.Map<LogBook>(logBookVM);

                _myContext.LogBooks.Add(logBook);
                _myContext.SaveChanges();

                //Logbook training details
                LogBookTrainingDetail logBookTrainingDetail = _mapper.Map<LogBookTrainingDetail>(logBookVM.LogBookTrainingDetailVM);
                logBookTrainingDetail.LogBookId = logBook.Id;
                _myContext.LogBookTrainingDetails.Add(logBookTrainingDetail);
                _myContext.SaveChanges();

                //Logbook instrument details
                LogBookInstrument logBookInstrument = _mapper.Map<LogBookInstrument>(logBookVM.LogBookInstrumentVM);
                logBookInstrument.LogBookId = logBook.Id;
                _myContext.LogBookInstruments.Add(logBookInstrument);
                _myContext.SaveChanges();

                //Logbook instrument approaches details
                List<LogBookInstrumentApproach> logBookInstrumentApproachesList = _mapper.Map<List<LogBookInstrumentApproach>>(logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList);
                logBookInstrumentApproachesList.ForEach(p => { p.LogBookInstrumentId = logBookInstrument.Id; });
                _myContext.LogBookInstrumentApproaches.AddRange(logBookInstrumentApproachesList);
                _myContext.SaveChanges();

                //Logbook flight time details
                LogBookFlightTimeDetail logBookFlightTimeDetail = _mapper.Map<LogBookFlightTimeDetail>(logBookVM.LogBookFlightTimeDetailVM);
                logBookFlightTimeDetail.LogBookId = logBook.Id;
                _myContext.LogBookFlightTimeDetails.Add(logBookFlightTimeDetail);
                _myContext.SaveChanges();

                //Logbook passengers list
                List<LogBookCrewPassenger> logBookCrewPassengersList = _mapper.Map<List<LogBookCrewPassenger>>(logBookVM.LogBookCrewPassengersList);
                logBookCrewPassengersList.ForEach(p => { p.LogBookId = logBook.Id; });
                _myContext.LogBookCrewPassengers.AddRange(logBookCrewPassengersList);
                _myContext.SaveChanges();

                //Logbook photos list
                List<LogBookFlightPhoto> logBookFlightPhotosList = _mapper.Map<List<LogBookFlightPhoto>>(logBookVM.LogBookFlightPhotosList);
                logBookFlightPhotosList.ForEach(p => { p.LogBookId = logBook.Id; });
                _myContext.LogBookFlightPhotos.AddRange(logBookFlightPhotosList);
                _myContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
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

                    List<LogBookInstrumentApproach> logBookInstrumentApproachesList =  _myContext.LogBookInstrumentApproaches.Where(p => p.LogBookInstrumentId == logBookInstrument.Id).ToList();
                    logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList = _mapper.Map<List<LogBookInstrumentApproachVM>>(logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList);
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
    }
}
