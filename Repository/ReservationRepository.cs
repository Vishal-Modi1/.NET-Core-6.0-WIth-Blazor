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
                List<UpcomingFlight> upcomingFlights = (from aircraftSchedule in _myContext.AircraftSchedules
                                                        join aircraft in _myContext.Aircrafts
                                                        on aircraftSchedule.AircraftId equals aircraft.Id
                                                        join user in _myContext.Users
                                                        on aircraftSchedule.Member1Id equals user.Id
                                                        where aircraftSchedule.IsActive == true &&
                                                        aircraftSchedule.Member1Id == userId &&
                                                        aircraftSchedule.IsDeleted == false &&
                                                        aircraftSchedule.StartDateTime >= userCurrentTime
                                                        orderby aircraftSchedule.StartDateTime
                                                        select new UpcomingFlight()
                                                        {
                                                            Id = aircraftSchedule.Id,
                                                            StartDate = aircraftSchedule.StartDateTime,
                                                            EndDate = aircraftSchedule.EndDateTime,
                                                            ArrivalAirport = aircraftSchedule.ArrivalAirportName,
                                                            DepartureAirport = aircraftSchedule.DepartureAirportName,
                                                            Title = aircraftSchedule.ScheduleTitle,
                                                            Member1 = user.FirstName + " " + user.LastName,
                                                            TailNo = aircraft.TailNo,
                                                            CompanyId = aircraftSchedule.CompanyId

                                                        }).Take(5).ToList();

                return upcomingFlights;
            }
        }

        public List<UpcomingFlight> ListUpcomingFlightsByCompanyId(int companyId, DateTime userCurrentTime)
        {
            using (_myContext = new MyContext())
            {
                List<UpcomingFlight> upcomingFlights = (from aircraftSchedule in _myContext.AircraftSchedules
                                                        join aircraft in _myContext.Aircrafts
                                                        on aircraftSchedule.AircraftId equals aircraft.Id
                                                        join user in _myContext.Users
                                                        on aircraftSchedule.Member1Id equals user.Id
                                                        where aircraftSchedule.IsActive == true &&
                                                        aircraftSchedule.CompanyId == companyId &&
                                                        aircraftSchedule.IsDeleted == false &&
                                                        aircraftSchedule.StartDateTime >= userCurrentTime
                                                        orderby aircraftSchedule.StartDateTime
                                                        select new UpcomingFlight()
                                                        {
                                                            Id = aircraftSchedule.Id,
                                                            StartDate = aircraftSchedule.StartDateTime,
                                                            EndDate = aircraftSchedule.EndDateTime,
                                                            ArrivalAirport = aircraftSchedule.ArrivalAirportName,
                                                            DepartureAirport = aircraftSchedule.DepartureAirportName,
                                                            Title = aircraftSchedule.ScheduleTitle,
                                                            Member1 = user.FirstName + " " + user.LastName,
                                                            TailNo = aircraft.TailNo,
                                                            CompanyId = aircraftSchedule.CompanyId,
                                                            AircraftImage = aircraft.ImageName

                                                        }).Take(5).ToList();

                return upcomingFlights;
            }
        }

        public List<UpcomingFlight> ListUpcomingFlightsByAircraftId(long aircraftId, DateTime userCurrentTime)
        {
            using (_myContext = new MyContext())
            {
                List<UpcomingFlight> upcomingFlights = (from aircraftSchedule in _myContext.AircraftSchedules
                                                        join aircraft in _myContext.Aircrafts
                                                        on aircraftSchedule.AircraftId equals aircraft.Id
                                                        join user in _myContext.Users
                                                        on aircraftSchedule.Member1Id equals user.Id
                                                        where aircraftSchedule.IsActive == true &&
                                                        aircraftSchedule.AircraftId == aircraftId &&
                                                        aircraftSchedule.IsDeleted == false &&
                                                        aircraftSchedule.StartDateTime >= userCurrentTime
                                                        orderby aircraftSchedule.StartDateTime
                                                        select new UpcomingFlight()
                                                        {
                                                            Id = aircraftSchedule.Id,
                                                            StartDate = aircraftSchedule.StartDateTime,
                                                            EndDate = aircraftSchedule.EndDateTime,
                                                            ArrivalAirport = aircraftSchedule.ArrivalAirportName,
                                                            DepartureAirport = aircraftSchedule.DepartureAirportName,
                                                            Title = aircraftSchedule.ScheduleTitle,
                                                            Member1 = user.FirstName + " " + user.LastName,
                                                            TailNo = aircraft.TailNo,
                                                            CompanyId = aircraftSchedule.CompanyId,
                                                            PilotImage = user.ImageName

                                                        }).Take(5).ToList();

                return upcomingFlights;
            }
        }
    }
}
