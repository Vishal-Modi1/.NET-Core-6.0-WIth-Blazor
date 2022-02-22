using DataModels.Entities;
using DataModels.VM.Common;
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

    }
}
