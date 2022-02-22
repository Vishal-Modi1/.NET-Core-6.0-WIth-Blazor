using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class AircraftCategoryRepository : IAircraftCategoryRepository
    {
        private MyContext _myContext;

        public List<AircraftCategory> List()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftCategories.ToList();
            }
        }

        public AircraftCategory Create(AircraftCategory aircraftCategory)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftCategories.Add(aircraftCategory);

                return aircraftCategory;
            }
        }

        public AircraftCategory FindByCondition(Expression<Func<AircraftCategory, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftCategories.Where(predicate).FirstOrDefault();
            }
        }

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> aircraftCategory = (from category in _myContext.AircraftCategories
                                                     select new DropDownValues()
                                                     {
                                                         Id = category.Id,
                                                         Name = category.Name
                                                     }).ToList();

                return aircraftCategory;
            }
        }
    }
}
