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
                _myContext.Aircrafts.Add(airCraft);
                _myContext.SaveChanges();

                return airCraft;
            }
        }

        public void Delete(long id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                Aircraft airCraft = _myContext.Aircrafts.Where(p => p.Id == id).FirstOrDefault();

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
                Aircraft existingAircraft = _myContext.Aircrafts.Where(p => p.Id == airCraft.Id).FirstOrDefault();

                if (existingAircraft != null)
                {
                    existingAircraft.CompanyId = airCraft.CompanyId;
                    existingAircraft.TailNo = airCraft.TailNo;
                    existingAircraft.Year = airCraft.Year;
                    existingAircraft.AircraftMakeId = airCraft.AircraftMakeId;
                    existingAircraft.AircraftModelId = airCraft.AircraftModelId;
                    existingAircraft.AircraftCategoryId = airCraft.AircraftCategoryId;
                    existingAircraft.AircraftClassId = airCraft.AircraftClassId;
                    existingAircraft.FlightSimulatorClassId = airCraft.FlightSimulatorClassId;
                    existingAircraft.NoofEngines = airCraft.NoofEngines;
                    existingAircraft.NoofPropellers = airCraft.NoofPropellers;
                    existingAircraft.IsEngineshavePropellers = airCraft.IsEngineshavePropellers;
                    existingAircraft.IsEnginesareTurbines = airCraft.IsEnginesareTurbines;
                    existingAircraft.TrackOilandFuel = airCraft.TrackOilandFuel;
                    existingAircraft.IsIdentifyMeterMismatch = airCraft.IsIdentifyMeterMismatch;
                    existingAircraft.OwnerId = airCraft.OwnerId;
                    existingAircraft.IsLockedForUpdate = airCraft.IsLockedForUpdate;

                    existingAircraft.UpdatedBy = airCraft.UpdatedBy;
                    existingAircraft.UpdatedOn = airCraft.UpdatedOn;
                }

                _myContext.SaveChanges();

                return airCraft;
            }
        }

        public Aircraft FindByCondition(Expression<Func<Aircraft, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Aircrafts.Where(predicate).FirstOrDefault();
            }
        }

        public List<AircraftDataVM> List(AircraftDatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                string sql = $"EXEC dbo.GetAircraftsList '{ datatableParams.SearchText }', { datatableParams.Start }, {datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}',{datatableParams.CompanyId},{(datatableParams.IsActive == true ? 1 : 0)}";
                List<AircraftDataVM> airCraftList = _myContext.AircraftDataVMs.FromSqlRaw<AircraftDataVM>(sql).ToList();

                return airCraftList;
            }
        }

        public List<Aircraft> ListAllByCompanyId(int companyId)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Aircrafts.Where(p=> p.CompanyId == companyId && p.IsActive == true && p.IsDeleted == false).ToList();
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
                Aircraft existingAircraft = _myContext.Aircrafts.Where(p => p.Id == id).FirstOrDefault();

                if (existingAircraft != null)
                {
                    existingAircraft.ImageName = imageName;
                    _myContext.SaveChanges();

                    return true;
                }

                return false ;
            }
        }

        public List<DropDownLargeValues> ListDropDownValuesByCompanyId(int companyId)
        {
            using (_myContext = new MyContext())
            {
                List<DropDownLargeValues> aircraftsList = (from aircraft in _myContext.Aircrafts
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
                Aircraft existingAircraft = _myContext.Aircrafts.Where(p => p.Id == id).FirstOrDefault();

                if (existingAircraft != null)
                {
                    existingAircraft.AircraftStatusId = statusId;
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
                Aircraft existingAircraft = _myContext.Aircrafts.Where(p => p.Id == id).FirstOrDefault();

                if (existingAircraft != null)
                {
                    existingAircraft.IsLockedForUpdate = isLock;
                    _myContext.SaveChanges();

                    return true;
                }

                return false;
            }
        }
    }
}
