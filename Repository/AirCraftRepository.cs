using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;

namespace Repository
{
    public class AircraftRepository : IAircraftRepository
    {
        private MyContext _myContext;

        public Aircraft Create(Aircraft airCraft)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AirCrafts.Add(airCraft);
                _myContext.SaveChanges();

                return airCraft;
            }
        }

        public void Delete(long id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                Aircraft airCraft = _myContext.AirCrafts.Where(p => p.Id == id).FirstOrDefault();

                if (airCraft != null)
                {
                    airCraft.IsDeleted = true;
                    airCraft.DeletedBy = deletedBy;
                    airCraft.DeletedOn = DateTime.UtcNow;

                    _myContext.SaveChanges();
                }
            }
        }

        public Aircraft Edit(Aircraft airCraft)
        {
            using (_myContext = new MyContext())
            {
                Aircraft existingAirCraft = _myContext.AirCrafts.Where(p => p.Id == airCraft.Id).FirstOrDefault();

                if (existingAirCraft != null)
                {
                    existingAirCraft.CompanyId = airCraft.CompanyId;
                    existingAirCraft.TailNo = airCraft.TailNo;
                    existingAirCraft.Year = airCraft.Year;
                    existingAirCraft.AircraftMakeId = airCraft.AircraftMakeId;
                    existingAirCraft.AircraftModelId = airCraft.AircraftModelId;
                    existingAirCraft.AircraftCategoryId = airCraft.AircraftCategoryId;
                    existingAirCraft.AircraftClassId = airCraft.AircraftClassId;
                    existingAirCraft.FlightSimulatorClassId = airCraft.FlightSimulatorClassId;
                    existingAirCraft.NoofEngines = airCraft.NoofEngines;
                    existingAirCraft.NoofPropellers = airCraft.NoofPropellers;
                    existingAirCraft.IsEngineshavePropellers = airCraft.IsEngineshavePropellers;
                    existingAirCraft.IsEnginesareTurbines = airCraft.IsEnginesareTurbines;
                    existingAirCraft.TrackOilandFuel = airCraft.TrackOilandFuel;
                    existingAirCraft.IsIdentifyMeterMismatch = airCraft.IsIdentifyMeterMismatch;
                    existingAirCraft.OwnerId = airCraft.OwnerId;
                    existingAirCraft.IsLockedForUpdate = airCraft.IsLockedForUpdate;

                    existingAirCraft.UpdatedBy = airCraft.UpdatedBy;
                    existingAirCraft.UpdatedOn = airCraft.UpdatedOn;
                }

                _myContext.SaveChanges();

                return airCraft;
            }
        }

        public Aircraft FindByCondition(Expression<Func<Aircraft, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AirCrafts.Where(predicate).FirstOrDefault();
            }
        }

        public List<AircraftDataVM> List(AircraftDatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;

                string sql = $"EXEC dbo.GetAircraftsList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}',{datatableParams.CompanyId},{(datatableParams.IsActive == true ? 1 : 0)}";
                List<AircraftDataVM> airCraftList = _myContext.AircraftDataVMs.FromSqlRaw<AircraftDataVM>(sql).ToList();

                return airCraftList;
            }
        }

        public List<Aircraft> ListAllByCompanyId(int companyId)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AirCrafts.Where(p=> p.CompanyId == companyId && p.IsActive == true && p.IsDeleted == false).ToList();
            }
        }

        public List<FlightSimulatorClass> FlightSimulatorClassList()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.FlightSimulatorClasses.ToList();
            }
        }

        public List<DropDownValues> ListFlightSimulatorClassDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> flightSimulatorClassesList = (from flightSimulatorClass in _myContext.FlightSimulatorClasses
                                                            select new DropDownValues()
                                                            {
                                                                Id = flightSimulatorClass.Id,
                                                                Name = flightSimulatorClass.Name
                                                            }).ToList();

                return flightSimulatorClassesList;
            }
        }

        public bool UpdateImageName(long id, string imageName)
        {
            using (_myContext = new MyContext())
            {
                Aircraft existingAirCraft = _myContext.AirCrafts.Where(p => p.Id == id).FirstOrDefault();

                if (existingAirCraft != null)
                {
                    existingAirCraft.ImageName = imageName;
                    _myContext.SaveChanges();

                    return true;
                }

                return false ;
            }
        }

        public List<DropDownLargeValues> ListDropDownValues(int companyId)
        {
            using (_myContext = new MyContext())
            {
                List<DropDownLargeValues> aircraftsList = (from aircraft in _myContext.AirCrafts
                                                           where aircraft.IsDeleted == false
                                                           && aircraft.IsActive == true
                                                           && (companyId == 0 || (aircraft.CompanyId == companyId))
                                                           select new DropDownLargeValues()
                                                           {
                                                                Id = aircraft.Id,
                                                                Name = aircraft.TailNo
                                                           }).ToList();

                return aircraftsList;
            }
        }

        public bool UpdateStatus(long id, byte statusId)
        {
            using (_myContext = new MyContext())
            {
                Aircraft existingAirCraft = _myContext.AirCrafts.Where(p => p.Id == id).FirstOrDefault();

                if (existingAirCraft != null)
                {
                    existingAirCraft.AircraftStatusId = statusId;
                    _myContext.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public bool UpdateIsLockedForUpdate(long id, bool isLock)
        {
            using (_myContext = new MyContext())
            {
                Aircraft existingAirCraft = _myContext.AirCrafts.Where(p => p.Id == id).FirstOrDefault();

                if (existingAirCraft != null)
                {
                    existingAirCraft.IsLockedForUpdate = isLock;
                    _myContext.SaveChanges();

                    return true;
                }

                return false;
            }
        }
    }
}
