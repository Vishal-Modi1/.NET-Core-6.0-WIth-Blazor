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

        public ReservationService(IReservationRepository reservationRepository, ICompanyRepository companyRepository)
        {
            _reservationRepository = reservationRepository;
            _companyRepository = companyRepository;
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

        public CurrentResponse GetFiltersValue(int roleId)
        {
            try
            {
                ReservationFilterVM reservationFilterVM = new ReservationFilterVM();

                reservationFilterVM.Companies = _companyRepository.ListDropDownValues();

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
