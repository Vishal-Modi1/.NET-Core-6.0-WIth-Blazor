using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Location;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class LocationRepository : ILocationRepository
    {
        private MyContext _myContext;

        public Location Create(Location location)
        {
            using (_myContext = new MyContext())
            {
                _myContext.Locations.Add(location);
                _myContext.SaveChanges();

                return location;
            }
        }

        public void Delete(int id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                Location location = _myContext.Locations.Where(p => p.Id == id).FirstOrDefault();

                if (location != null)
                {
                    location.DeletedBy = deletedBy;
                    location.DeletedOn = DateTime.UtcNow;

                    _myContext.SaveChanges();
                }
            }
        }

        public Location Edit(Location location)
        {
            using (_myContext = new MyContext())
            {
                Location existingLocation = _myContext.Locations.Where(p => p.Id == location.Id).FirstOrDefault();

                if (existingLocation != null)
                {
                    existingLocation.PhysicalAddress = location.PhysicalAddress;
                    existingLocation.AirportCode = location.AirportCode;
                    existingLocation.TimezoneId = location.TimezoneId;

                    existingLocation.UpdatedBy = location.UpdatedBy;
                    existingLocation.UpdatedOn = location.UpdatedOn;
                }

                _myContext.SaveChanges();

                return location;
            }
        }

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> locationList = (from location in _myContext.Locations
                                                     join timezone in _myContext.Timezones
                                                     on location.TimezoneId equals timezone.Id
                                                     where location.DeletedBy != null
                                                     select new DropDownValues()
                                                     {
                                                         Id = location.Id,
                                                         Name = timezone.TimezoneName
                                                     }).ToList();

                return locationList;
            }
        }

        public List<LocationDataVM> List(DatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
                List<LocationDataVM> list;
                string sql = $"EXEC dbo.GetLocationsList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length}," +
                    $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";

                list = _myContext.LocationsList.FromSqlRaw<LocationDataVM>(sql).ToList();

                return list;
            }
        }

        public Location FindByCondition(Expression<Func<Location, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Locations.Where(predicate).FirstOrDefault();
            }
        }
    }
}
