using DataModels.Entities;
using DataModels.VM.Aircraft.AircraftStatusLog;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class AircraftStatusLogService : BaseService, IAircraftStatusLogService
    {
        private readonly IAircraftStatusLogRepository _aircraftStatusLogRepository;

        public AircraftStatusLogService(IAircraftStatusLogRepository aircraftStatusLogRepository)
        {
            _aircraftStatusLogRepository = aircraftStatusLogRepository;
        }

        public CurrentResponse Create(AircraftStatusLogVM aircraftStatusLogVM)
        {
            AircraftStatusLog aircraftStatusLog = ToDataObject(aircraftStatusLogVM);

            try
            {
                aircraftStatusLog = _aircraftStatusLogRepository.Create(aircraftStatusLog);

                //companyVM = ToBusinessObject(company);

                CreateResponse(aircraftStatusLogVM, HttpStatusCode.OK, "Aircraft status log added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(int id, long deletedBy)
        {
            try
            {
                _aircraftStatusLogRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Aircraft status log deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(AircraftStatusLogVM aircraftStatusLogVM)
        {
            AircraftStatusLog aircraftStatusLog = ToDataObject(aircraftStatusLogVM);

            try
            {
                aircraftStatusLog = _aircraftStatusLogRepository.Edit(aircraftStatusLog);

                //companyVM = ToBusinessObject(company);

                CreateResponse(aircraftStatusLogVM, HttpStatusCode.OK, "Aircraft status log updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(AircraftStatusLogDatatableParams datatableParams)
        {
            try
            {
                List<AircraftStatusLogDataVM> logsList = _aircraftStatusLogRepository.List(datatableParams);

                CreateResponse(logsList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        #region Object Mapper

        private AircraftStatusLog ToDataObject(AircraftStatusLogVM aircraftStatusLogVM)
        {
            AircraftStatusLog aircraftStatusLog = new AircraftStatusLog();

            aircraftStatusLog.AircraftId = aircraftStatusLogVM.AircraftId;
            aircraftStatusLog.AircraftStatusId = aircraftStatusLogVM.AircraftStatusId;
            aircraftStatusLog.Reason = aircraftStatusLogVM.Reason;

            if (aircraftStatusLogVM.Id == 0)
            {
                aircraftStatusLog.CreatedOn = DateTime.UtcNow;
                aircraftStatusLog.CreatedBy = aircraftStatusLogVM.CreatedBy;
            }
            else
            {
                aircraftStatusLog.UpdatedOn = DateTime.UtcNow;
                aircraftStatusLog.UpdatedBy = aircraftStatusLogVM.UpdatedBy;
            }

            return aircraftStatusLog;
        }

        #endregion

    }
}
