using DataModels.VM.Common;
using DataModels.VM.Reservation;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

namespace Service
{
    public class ReservationService : BaseService, IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAircraftRepository _aircraftRepository;

        public ReservationService(IReservationRepository reservationRepository, 
            ICompanyRepository companyRepository, IAircraftRepository aircraftRepository)
        {
            _reservationRepository = reservationRepository;
            _companyRepository = companyRepository;
            _aircraftRepository = aircraftRepository;
        }

        public CurrentResponse List(ReservationDataTableParams datatableParams)
        {
            try
            {
                var data = _reservationRepository.List(datatableParams);

                CreateResponse(data, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetFiltersValue(int roleId, int companyId)
        {
            try
            {
                ReservationFilterVM reservationFilterVM = new ReservationFilterVM();

                reservationFilterVM.Companies = _companyRepository.ListDropDownValues();
                reservationFilterVM.Aircrafts = _aircraftRepository.ListDropDownValues(companyId);

                CreateResponse(reservationFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new ReservationFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
