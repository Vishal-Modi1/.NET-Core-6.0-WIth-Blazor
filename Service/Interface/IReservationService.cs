using DataModels.VM.Common;
using DataModels.VM.Reservation;

namespace Service.Interface
{
    public interface IReservationService
    {
        CurrentResponse List(ReservationDataTableParams datatableParams);

        CurrentResponse GetFiltersValue(int roleId, int compamyId);

        CurrentResponse ListUpcomingFlightsByUserId(long userId);

        CurrentResponse ListUpcomingFlightsByCompanyId(int companyId);

        CurrentResponse ListUpcomingFlightsByAircraftId(long aircraftId);
    }
}
