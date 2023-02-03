using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.LogBook;
using DataModels.Entities;

namespace Service
{
    public class LogBookService : BaseService, ILogBookService
    {
        private readonly ILogBookRepository _logBookRepository;   

        public LogBookService(ILogBookRepository logBookRepository)
        {
            _logBookRepository = logBookRepository;
        }

        public CurrentResponse Create(LogBookVM logBook)
        {
            try
            {
                logBook.CreatedOn = DateTime.UtcNow;
                logBook =  _logBookRepository.Create(logBook);

                CreateResponse(logBook, HttpStatusCode.OK, "Logbook added successfully");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindById(long id)
        {
            try
            {
                LogBookVM logBookVM = _logBookRepository.FindById(id);

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
    }
}
