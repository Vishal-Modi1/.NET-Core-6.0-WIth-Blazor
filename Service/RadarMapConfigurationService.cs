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
    public class RadarMapConfigurationService : BaseService, IRadarMapConfigurationService
    {
        private readonly IAircraftLiveTrackerMapConfigurationRepository _aircraftLiveTrackerMapConfigurationRepository;
        private readonly IRadarMapConfigurationRepository _radarMapConfigurationRepository;
        private readonly IVFRMapConfigurationRepository _vFRMapConfigurationRepository;
        private readonly IWindyMapConfigurationRepository _windyMapConfigurationRepository;
        private readonly IMapper _mapper;

        public RadarMapConfigurationService(IAircraftLiveTrackerMapConfigurationRepository
            aircraftLiveTrackerMapConfigurationRepository, IMapper mapper,
            IRadarMapConfigurationRepository radarMapConfigurationRepository,
            IWindyMapConfigurationRepository windyMapConfigurationRepository, IVFRMapConfigurationRepository vFRMapConfigurationRepository)
        {
            _radarMapConfigurationRepository = radarMapConfigurationRepository;
            _windyMapConfigurationRepository = windyMapConfigurationRepository;
            _aircraftLiveTrackerMapConfigurationRepository = aircraftLiveTrackerMapConfigurationRepository;
            _mapper = mapper;
            _vFRMapConfigurationRepository = vFRMapConfigurationRepository;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                RadarMapConfigurationVM radarMapConfigurationVM = new();
                RadarMapConfiguration data = _radarMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

                if (data is not null)
                {
                    radarMapConfigurationVM = _mapper.Map<RadarMapConfigurationVM>(data);
                }

                CreateResponse(radarMapConfigurationVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(RadarMapConfigurationVM radarMapConfigurationVM)
        {
            try
            {
                RadarMapConfiguration radarMapConfiguration = _mapper.Map<RadarMapConfiguration>(radarMapConfigurationVM);
                _radarMapConfigurationRepository.SetDefault(radarMapConfiguration);
                
                if (radarMapConfigurationVM.IsApplyToAll)
                {
                    _aircraftLiveTrackerMapConfigurationRepository.SetDefault(radarMapConfiguration.UserId, radarMapConfiguration.Height, radarMapConfiguration.Width);
                    _windyMapConfigurationRepository.SetDefault(radarMapConfiguration.UserId, radarMapConfiguration.Height, radarMapConfiguration.Width);
                    _vFRMapConfigurationRepository.SetDefault(radarMapConfiguration.UserId, radarMapConfiguration.Height, radarMapConfiguration.Width);
                }
                
                radarMapConfigurationVM = _mapper.Map<RadarMapConfigurationVM>(radarMapConfiguration);

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
