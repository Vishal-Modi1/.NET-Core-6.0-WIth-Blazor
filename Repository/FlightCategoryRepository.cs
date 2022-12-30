using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
//using DataModels.VM.FlightCategory;
using DataModels.VM.Common;

namespace Repository
{
    public class FlightCategoryRepository : BaseRepository<FlightCategory>, IFlightCategoryRepository
    {
        private readonly MyContext _myContext;

        public FlightCategoryRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public List<DropDownValues> ListDropDownValuesByCompanyId(int companyId)
        {
            List<DropDownValues> flightCategoryList = _myContext.FlightCategories.Where(p => p.CompanyId == companyId || p.CompanyId == null).
                                               Select(n => new DropDownValues
                                               { Id = n.Id, Name = n.Name }).ToList();

            return flightCategoryList;
        }

        public List<FlightCategory> ListByCompanyId(int companyId)
        {
            List<FlightCategory> flightCategoryList = _myContext.FlightCategories.Where(p => p.CompanyId == companyId || p.CompanyId == null).ToList();

            return flightCategoryList;
        }

        public FlightCategory Edit(FlightCategory flightCategory)
        {
            FlightCategory existingFlightCategory = _myContext.FlightCategories.Where(p => p.Id == flightCategory.Id).FirstOrDefault();

            if (existingFlightCategory != null)
            {
                existingFlightCategory.Name = flightCategory.Name;
                existingFlightCategory.Color = flightCategory.Color;

                _myContext.SaveChanges();
            }

            return flightCategory;
        }

        public void Delete(int id)
        {
            FlightCategory flightCategory = _myContext.FlightCategories.Where(p => p.Id == id).FirstOrDefault();

            if (flightCategory != null)
            {
                _myContext.FlightCategories.Remove(flightCategory);
                _myContext.SaveChanges();
            }
        }
    }
}
