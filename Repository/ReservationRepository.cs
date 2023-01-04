using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using DataModels.VM.Reservation;
using System;

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

                if (datatableParams.ReservationType.ToString() != "0")
                {
                    reservationType = datatableParams.ReservationType.ToString();
                }

                if (datatableParams.StartDate != null)
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
                    $"{datatableParams.AircraftId.GetValueOrDefault()},'{reservationType}','{datatableParams.DepartureAirportId}'" +
                    $",'{datatableParams.ArrivalAirportId}'";

                list = _myContext.ReservationDataVM.FromSqlRaw<ReservationDataVM>(sql).ToList();

                return list;
            }
        }

        public List<UpcomingFlight> ListUpcomingFlightsByUserId(long userId, DateTime userCurrentTime)
        {
            using (_myContext = new MyContext())
            {
                List<UpcomingFlight> upcomingFlights = _myContext.AircraftSchedules.Where(p => p.Member1Id == userId
                && p.StartDateTime >= userCurrentTime && p.IsDeleted == false && p.IsActive == true)
                    .OrderBy(o => o.StartDateTime).Take(5).Select(l => new UpcomingFlight()
                    { Id = l.Id, StartDate = l.StartDateTime, Title = l.ScheduleTitle }).ToList();

                return upcomingFlights;
            }
        }

        public List<UpcomingFlight> ListUpcomingFlightsByCompanyId(int companyId, DateTime userCurrentTime)
        {
            using (_myContext = new MyContext())
            {
                List<UpcomingFlight> upcomingFlights = _myContext.AircraftSchedules.Where(p => p.CompanyId == companyId
                && p.StartDateTime >= userCurrentTime && p.IsDeleted == false && p.IsActive == true)
                    .OrderBy(o => o.StartDateTime).Take(5).Select(l => new UpcomingFlight()
                    { Id = l.Id, StartDate = l.StartDateTime, Title = l.ScheduleTitle }).ToList();

                return upcomingFlights;
            }
        }

        public List<UpcomingFlight> ListUpcomingFlightsByAircraftId(long aircraftId, DateTime userCurrentTime)
        {
            using (_myContext = new MyContext())
            {
                List<UpcomingFlight> upcomingFlights = _myContext.AircraftSchedules.Where(p => p.AircraftId == aircraftId
                && p.StartDateTime >= userCurrentTime && p.IsDeleted == false && p.IsActive == true)
                    .OrderBy(o => o.StartDateTime).Take(5).Select(l => new UpcomingFlight()
                    { Id = l.Id, StartDate = l.StartDateTime, Title = l.ScheduleTitle }).ToList();

                return upcomingFlights;
            }
        }
    }
}
