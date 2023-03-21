using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Weather;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

namespace Service
{
    public class NOAARadarMapConfigurationService : BaseService, INOAARadarMapConfigurationService
    {
        private readonly IAircraftLiveTrackerMapConfigurationRepository _aircraftLiveTrackerMapConfigurationRepository;
        private readonly IRadarMapConfigurationRepository _radarMapConfigurationRepository;
        private readonly INOAARadarMapConfigurationRepository _nOAARadarMapConfigurationRepository;
        private readonly IVFRMapConfigurationRepository _vFRMapConfigurationRepository;
        private readonly IWindyMapConfigurationRepository _windyMapConfigurationRepository;
        private readonly IMapper _mapper;

        public NOAARadarMapConfigurationService(IAircraftLiveTrackerMapConfigurationRepository
            aircraftLiveTrackerMapConfigurationRepository, IMapper mapper,
            IRadarMapConfigurationRepository radarMapConfigurationRepository,
            IWindyMapConfigurationRepository windyMapConfigurationRepository, IVFRMapConfigurationRepository vFRMapConfigurationRepository,
            INOAARadarMapConfigurationRepository nOAARadarMapConfigurationRepository)
        {
            _radarMapConfigurationRepository = radarMapConfigurationRepository;
            _windyMapConfigurationRepository = windyMapConfigurationRepository;
            _aircraftLiveTrackerMapConfigurationRepository = aircraftLiveTrackerMapConfigurationRepository;
            _mapper = mapper;
            _vFRMapConfigurationRepository = vFRMapConfigurationRepository;
            _nOAARadarMapConfigurationRepository = nOAARadarMapConfigurationRepository;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                NOAARadarMapConfigurationVM noaaradarMapConfigurationVM = new();
                NOAARadarMapConfiguration data = _nOAARadarMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

                if (data is not null)
                {
                    noaaradarMapConfigurationVM = _mapper.Map<NOAARadarMapConfigurationVM>(data);
                }

                CreateResponse(noaaradarMapConfigurationVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(NOAARadarMapConfigurationVM nOAARadarMapConfigurationVM)
        {
            try
            {
                NOAARadarMapConfiguration noaaRadarMapConfiguration = _mapper.Map<NOAARadarMapConfiguration>(nOAARadarMapConfigurationVM);
                _nOAARadarMapConfigurationRepository.SetDefault(noaaRadarMapConfiguration);
                
                if (nOAARadarMapConfigurationVM.IsApplyToAll)
                {
                    _aircraftLiveTrackerMapConfigurationRepository.SetDefault(noaaRadarMapConfiguration.UserId, noaaRadarMapConfiguration.Height, noaaRadarMapConfiguration.Width);
                    _windyMapConfigurationRepository.SetDefault(noaaRadarMapConfiguration.UserId, noaaRadarMapConfiguration.Height, noaaRadarMapConfiguration.Width);
                    _vFRMapConfigurationRepository.SetDefault(noaaRadarMapConfiguration.UserId, noaaRadarMapConfiguration.Height, noaaRadarMapConfiguration.Width);
                    _radarMapConfigurationRepository.SetDefault(noaaRadarMapConfiguration.UserId, noaaRadarMapConfiguration.Height, noaaRadarMapConfiguration.Width);
                }

                nOAARadarMapConfigurationVM = _mapper.Map<NOAARadarMapConfigurationVM>(noaaRadarMapConfiguration);

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
