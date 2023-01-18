using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Weather;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

namespace Service
{
    public class WindyMapConfigurationService : BaseService, IWindyMapConfigurationService
    {
        private readonly IAircraftLiveTrackerMapConfigurationRepository _aircraftLiveTrackerMapConfigurationRepository;
        private readonly IRadarMapConfigurationRepository _radarMapConfigurationRepository;
        private readonly IVFRMapConfigurationRepository _vFRMapConfigurationRepository;
        private readonly IWindyMapConfigurationRepository _windyMapConfigurationRepository;
        private readonly IMapper _mapper;

        public WindyMapConfigurationService(IAircraftLiveTrackerMapConfigurationRepository
            aircraftLiveTrackerMapConfigurationRepository, IMapper mapper,
            IRadarMapConfigurationRepository radarMapConfigurationRepository,
            IWindyMapConfigurationRepository windyMapConfigurationRepository, IVFRMapConfigurationRepository vFRMapConfigurationRepository)
        {
            _windyMapConfigurationRepository = windyMapConfigurationRepository;
            _radarMapConfigurationRepository = radarMapConfigurationRepository;
            _aircraftLiveTrackerMapConfigurationRepository = aircraftLiveTrackerMapConfigurationRepository;
            _mapper = mapper;
            _vFRMapConfigurationRepository = vFRMapConfigurationRepository;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                WindyMapConfigurationVM windyMapConfigurationVM = new();
                WindyMapConfiguration data = _windyMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

                if (data is not null)
                {
                    windyMapConfigurationVM = _mapper.Map<WindyMapConfigurationVM>(data);
                }

                CreateResponse(windyMapConfigurationVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(WindyMapConfigurationVM windyMapConfigurationVM)
        {
            try
            {
                WindyMapConfiguration windyMapConfiguration = _mapper.Map<WindyMapConfiguration>(windyMapConfigurationVM);
                _windyMapConfigurationRepository.SetDefault(windyMapConfiguration);
                
                if (windyMapConfigurationVM.IsApplyToAll)
                {
                    _radarMapConfigurationRepository.SetDefault(windyMapConfiguration.UserId, windyMapConfiguration.Height, windyMapConfiguration.Width);
                    _vFRMapConfigurationRepository.SetDefault(windyMapConfiguration.UserId, windyMapConfiguration.Height, windyMapConfiguration.Width);
                    _aircraftLiveTrackerMapConfigurationRepository.SetDefault(windyMapConfiguration.UserId, windyMapConfiguration.Height, windyMapConfiguration.Width);
                }

                windyMapConfigurationVM = _mapper.Map<WindyMapConfigurationVM>(windyMapConfiguration);

                CreateResponse(windyMapConfigurationVM, HttpStatusCode.OK, "Default value updated");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        
    }
}
