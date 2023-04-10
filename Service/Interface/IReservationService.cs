using DataModels.VM.Common;
using DataModels.VM.Reservation;
using System;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IReservationService
    {
        CurrentResponse List(ReservationDataTableParams datatableParams);

        CurrentResponse GetFiltersValue(int roleId, int compamyId);

        CurrentResponse ListUpcomingFlightsByUserId(long userId, DateTime userTime);

        CurrentResponse ListUpcomingFlightsByCompanyId(int companyId, DateTime userTime);

        CurrentResponse ListUpcomingFlightsByAircraftId(long aircraftId, DateTime userTime);

        List<DropDownValues> ListReservationTypes();
    }
}
