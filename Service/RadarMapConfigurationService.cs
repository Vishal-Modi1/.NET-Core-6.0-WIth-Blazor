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
        private readonly IRadarMapConfigurationRepository _radarMapConfigurationRepository;
        private readonly IMapper _mapper;

        public RadarMapConfigurationService(IRadarMapConfigurationRepository 
            radarMapConfigurationRepository, IMapper mapper)
        {
            _radarMapConfigurationRepository = radarMapConfigurationRepository;
            _mapper = mapper;
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
