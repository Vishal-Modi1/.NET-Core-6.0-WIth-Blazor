using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using DataModels.VM.Reservation;

namespace Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private MyContext _myContext;

        public List<ReservationDataVM> List(ReservationDataTableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                List<ReservationDataVM> list;
                string startDate = "", endDate = "";
                string reservationType = "";

                if(datatableParams.ReservationType.ToString() != "0")
                {
                    reservationType = datatableParams.ReservationType.ToString();
                }

                if(datatableParams.StartDate != null)
                {
                    startDate = datatableParams.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (datatableParams.EndDate != null)
                {
                    endDate = datatableParams.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

                string sql = $"EXEC dbo.GetReservationsList '{ datatableParams.SearchText }', { datatableParams.Start }, " +
                    $"{datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', " +
                    $"{datatableParams.CompanyId},'{startDate}','{endDate}',{datatableParams.UserId.GetValueOrDefault()}," +
                    $"{datatableParams.AircraftId.GetValueOrDefault()},'{reservationType}'";

                list = _myContext.ReservationDataVM.FromSqlRaw<ReservationDataVM>(sql).ToList();

                return list;
            }
        }
    }
}
