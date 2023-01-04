using DataModels.VM.Reservation;
using System;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IReservationRepository
    {
        List<ReservationDataVM> List(ReservationDataTableParams datatableParams);
        List<UpcomingFlight> ListUpcomingFlightsByUserId(long userId, DateTime userCurrentTime);
        List<UpcomingFlight> ListUpcomingFlightsByCompanyId(int companyId, DateTime userCurrentTime);
        List<UpcomingFlight> ListUpcomingFlightsByAircraftId(long aircraftId, DateTime userCurrentTime);
    }
}
