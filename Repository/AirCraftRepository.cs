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

        public AirCraft Create(AirCraft airCraft)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AirCrafts.Add(airCraft);
                _myContext.SaveChanges();

                return airCraft;
            }
        }

        public void Delete(int id)
        {
            using (_myContext = new MyContext())
            {
                AirCraft airCraft = _myContext.AirCrafts.Where(p => p.Id == id).FirstOrDefault();

                if (airCraft != null)
                {
                    _myContext.AirCrafts.Remove(airCraft);
                    _myContext.SaveChanges();
                }
            }
        }

        public AirCraft Edit(AirCraft airCraft)
        {
            using (_myContext = new MyContext())
            {
                AirCraft existingAirCraft = _myContext.AirCrafts.Where(p => p.Id == airCraft.Id).FirstOrDefault();

                if (existingAirCraft != null)
                {
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
                }

                _myContext.SaveChanges();

                return airCraft;
            }
        }

        public AirCraft FindByCondition(Expression<Func<AirCraft, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AirCrafts.Where(predicate).FirstOrDefault();
            }
        }

        public List<AirCraft> List(AircraftFilterVM aircraftFilterVM)
        {
            using (_myContext = new MyContext())
            {
                //int pageNo = datatableParams.Start > 10 ? (datatableParams.Start / datatableParams.Length) : 1;


                //string sql = $"EXEC dbo.GetAirCraftList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";

                // list = _myContext.AirCraftVM.FromSqlRaw<AirCraftVM>(sql).ToList();

                List<AirCraft> airCraftList = new List<AirCraft>();

                if (!string.IsNullOrWhiteSpace(aircraftFilterVM.TailNo))
                {
                    airCraftList = _myContext.AirCrafts.Where(p => p.IsActive == aircraftFilterVM.IsActive && p.IsDeleted == false && p.TailNo.Contains(aircraftFilterVM.TailNo)).ToList();
                }
                else if (aircraftFilterVM.CompanyId > 0)
                {
                    airCraftList = _myContext.AirCrafts.Where(p => p.IsActive == aircraftFilterVM.IsActive && p.IsDeleted == false && p.CompanyId == aircraftFilterVM.CompanyId).ToList();
                }
                else
                {
                    airCraftList = _myContext.AirCrafts.Where(p => p.IsActive == aircraftFilterVM.IsActive && p.IsDeleted == false).ToList();
                }

                return airCraftList;
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

        public bool UpdateImageName(int id, string imageName)
        {
            using (_myContext = new MyContext())
            {
                AirCraft existingAirCraft = _myContext.AirCrafts.Where(p => p.Id == id).FirstOrDefault();

                if (existingAirCraft != null)
                {
                    existingAirCraft.ImageName = imageName;
                    _myContext.SaveChanges();

                    return true;
                }

                return false ;
            }
        }
    }
}
