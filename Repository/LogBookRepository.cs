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
                List<LogBookFlightPhoto> logBookFlightPhotosList = _mapper.Map<List<LogBookFlightPhoto>>(logBookVM.logBookFlightPhotosList);
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
                LogBookVM logBookVM;

                _myContext.Database.OpenConnection();

                var param = new SqlParameter("@id", id);

                var cmd = _myContext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = $"dbo.GetLogBookDetailsById";
                cmd.Parameters.Add(param);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var reader = cmd.ExecuteReader();

                var dt = new DataTable();
                dt.Load(reader);

                var rows = dt.AsEnumerable();

                // Read Blogs from the first result set
                logBookVM = rows.Select(dr => new LogBookVM()).FirstOrDefault();


                return logBookVM;

                _myContext.Database.CloseConnection();
            }
            catch(Exception exc)
            {
                return new LogBookVM();
            }
        }
    }
}
