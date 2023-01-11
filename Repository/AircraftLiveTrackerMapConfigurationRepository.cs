﻿using DataModels.Entities;
using Repository.Interface;
using System.Linq;

namespace Repository
{
    public class AircraftLiveTrackerMapConfigurationRepository : BaseRepository<AircraftLiveTrackerMapConfiguration>, IAircraftLiveTrackerMapConfigurationRepository
    {
        private readonly MyContext _myContext;

        public AircraftLiveTrackerMapConfigurationRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void SetDefault(AircraftLiveTrackerMapConfiguration aircraftLiveTrackerMapConfiguration)
        {
            AircraftLiveTrackerMapConfiguration existingAircraftLiveTrackerMapConfiguration = _myContext.AircraftLiveTrackerMapConfigurations.Where(p => p.UserId == aircraftLiveTrackerMapConfiguration.UserId).FirstOrDefault();

            if (existingAircraftLiveTrackerMapConfiguration != null)
            {
                existingAircraftLiveTrackerMapConfiguration.Width = aircraftLiveTrackerMapConfiguration.Width;
                existingAircraftLiveTrackerMapConfiguration.Height = aircraftLiveTrackerMapConfiguration.Height;
            }
            else
            {
                _myContext.AircraftLiveTrackerMapConfigurations.Add(aircraftLiveTrackerMapConfiguration);
            }
         
            _myContext.SaveChanges();
        }
    }
}
