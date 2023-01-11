using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using AutoMapper;
using System.Net;
using DataModels.VM.Weather;

namespace Service
{
    public class AircraftLiveTrackerMapConfigurationService : BaseService, IAircraftLiveTrackerMapConfigurationService
    {
        private readonly IAircraftLiveTrackerMapConfigurationRepository _aircraftLiveTrackerMapConfigurationRepository;
        private readonly IMapper _mapper;

        public AircraftLiveTrackerMapConfigurationService(IAircraftLiveTrackerMapConfigurationRepository
            aircraftLiveTrackerMapConfigurationRepository, IMapper mapper)
        {
            _aircraftLiveTrackerMapConfigurationRepository = aircraftLiveTrackerMapConfigurationRepository;
            _mapper = mapper;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                AircraftLiveTrackerMapConfigurationVM aircraftLiveTrackerMapConfigurationVM = new(); 
                AircraftLiveTrackerMapConfiguration data = _aircraftLiveTrackerMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

                if (data is not null)
                {
                    aircraftLiveTrackerMapConfigurationVM = _mapper.Map<AircraftLiveTrackerMapConfigurationVM>(data);
                }

                CreateResponse(aircraftLiveTrackerMapConfigurationVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(AircraftLiveTrackerMapConfigurationVM aircraftLiveTrackerMapConfigurationVM)
        {
            try
            {
                AircraftLiveTrackerMapConfiguration aircraftLiveTrackerMapConfiguration = _mapper.Map<AircraftLiveTrackerMapConfiguration>(aircraftLiveTrackerMapConfigurationVM);
                _aircraftLiveTrackerMapConfigurationRepository.SetDefault(aircraftLiveTrackerMapConfiguration);
                aircraftLiveTrackerMapConfigurationVM = _mapper.Map<AircraftLiveTrackerMapConfigurationVM>(aircraftLiveTrackerMapConfiguration);

                CreateResponse(true, HttpStatusCode.OK, "Default value updated");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
