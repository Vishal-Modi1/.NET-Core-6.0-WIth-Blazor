using DataModels.Entities;
using DataModels.VM.AircraftMake;
using DataModels.VM.Common;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class AircraftMakeRepository : IAircraftMakeRepository
    {
        private MyContext _myContext;

        public List<AircraftMake> List()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftMakes.ToList();
            }
        }

        public AircraftMake Create(AircraftMake aircraftMake)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftMakes.Add(aircraftMake);
                _myContext.SaveChanges();

                return aircraftMake;
            }
        }

        public AircraftMake FindByCondition(Expression<Func<AircraftMake, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftMakes.Where(predicate).FirstOrDefault();
            }
        }

        public bool IsAlreadyUsed(int id)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Aircrafts.Where(p => p.AircraftMakeId == id).Any();
            }
        }

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> aircraftMakeList = (from aircraftMake in _myContext.AircraftMakes
                                                         select new DropDownValues()
                                                         {
                                                             Id = aircraftMake.Id,
                                                             Name = aircraftMake.Name
                                                         }).ToList();

                return aircraftMakeList;
            }
        }

        public List<AircraftMakeDataVM> List(DatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                List<AircraftMakeDataVM> list;

                string sql = $"EXEC dbo.GetAircraftMakesList '{ datatableParams.SearchText }', { datatableParams.Start }, {datatableParams.Length}," +
                    $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";

                list = _myContext.AircraftMakesList.FromSqlRaw<AircraftMakeDataVM>(sql).ToList();

                return list;

            }
        }

        public void Delete(int id)
        {
            using (_myContext = new MyContext())
            {
                AircraftMake aircraftMake = _myContext.AircraftMakes.Where(p => p.Id == id).FirstOrDefault();

                if (aircraftMake != null)
                {
                    _myContext.AircraftMakes.Remove(aircraftMake);
                    _myContext.SaveChanges();
                }
            }
        }

        public AircraftMake Edit(AircraftMake aircraftMake)
        {
            using (_myContext = new MyContext())
            {
                AircraftMake existingAircraftMake = _myContext.AircraftMakes.Where(p => p.Id == aircraftMake.Id).FirstOrDefault();

                if (existingAircraftMake != null)
                {
                    existingAircraftMake.Name = aircraftMake.Name;
                    _myContext.SaveChanges();
                }

                return aircraftMake;
            }
        }
    }
}
