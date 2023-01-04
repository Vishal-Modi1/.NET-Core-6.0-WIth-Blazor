using DataModels.VM.Reservation;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IReservationRepository
    {
        List<ReservationDataVM> List(ReservationDataTableParams datatableParams);
        List<UpcomingFlight> ListUpcomingFlightsByUserId(long userId);
        List<UpcomingFlight> ListUpcomingFlightsByCompanyId(int companyId);
        List<UpcomingFlight> ListUpcomingFlightsByAircraftId(long aircraftId);
    }
}
