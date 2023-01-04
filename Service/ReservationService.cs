using DataModels.VM.Common;
using DataModels.VM.Reservation;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class ReservationService : BaseService, IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAircraftScheduleRepository _aircraftScheduleRepository; 

        public ReservationService(IReservationRepository reservationRepository, 
            ICompanyRepository companyRepository, IAircraftRepository aircraftRepository,
            IAircraftScheduleRepository aircraftScheduleRepository)
        {
            _reservationRepository = reservationRepository;
            _companyRepository = companyRepository;
            _aircraftRepository = aircraftRepository;
            _aircraftScheduleRepository = aircraftScheduleRepository;
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

                reservationFilterVM.DepartureAirportsList = _aircraftScheduleRepository.ListAirportDropDownValues();
                reservationFilterVM.ArrivalAirportsList = _aircraftScheduleRepository.ListAirportDropDownValues();

                CreateResponse(reservationFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListUpcomingFlightsByUserId(long userId)
        {
            try
            {
                List<UpcomingFlight> listUpcomingFlights = _reservationRepository.ListUpcomingFlightsByUserId(userId);
                CreateResponse(listUpcomingFlights, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListUpcomingFlightsByCompanyId(int companyId)
        {
            try
            {
                List<UpcomingFlight> listUpcomingFlights = _reservationRepository.ListUpcomingFlightsByCompanyId(companyId);
                CreateResponse(listUpcomingFlights, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListUpcomingFlightsByAircraftId(long aircraftId)
        {
            try
            {
                List<UpcomingFlight> listUpcomingFlights = _reservationRepository.ListUpcomingFlightsByAircraftId(aircraftId);
                CreateResponse(listUpcomingFlights, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
