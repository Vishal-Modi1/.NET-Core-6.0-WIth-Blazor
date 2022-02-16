﻿using Microsoft.EntityFrameworkCore;
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
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
                List<ReservationDataVM> list;
                string startDate = "", endDate = "";

                if(datatableParams.StartDate != null)
                {
                    startDate = datatableParams.StartDate.Value.ToString("yyyy-MM-dd hh:mm:ss");
                }

                if (datatableParams.EndDate != null)
                {
                    endDate = datatableParams.EndDate.Value.ToString("yyyy-MM-dd hh:mm:ss");
                }

                string sql = $"EXEC dbo.GetReservationList '{ datatableParams.SearchText }', { pageNo }, " +
                    $"{datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', " +
                    $"{datatableParams.CompanyId},'{startDate}','{endDate}'";

                list = _myContext.ReservationDataVM.FromSqlRaw<ReservationDataVM>(sql).ToList();

                return list;
            }
        }
    }
}