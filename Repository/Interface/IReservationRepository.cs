using DataModels.VM.Reservation;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IReservationRepository
    {
        List<ReservationDataVM> List(ReservationDataTableParams datatableParams);
    }
}
