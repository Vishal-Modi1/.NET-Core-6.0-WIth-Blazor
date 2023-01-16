using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Weather;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VFRMapConfigurationService : BaseService, IVFRMapConfigurationService
    {
        private readonly IAircraftLiveTrackerMapConfigurationRepository _aircraftLiveTrackerMapConfigurationRepository;
        private readonly IVFRMapConfigurationRepository _vFRMapConfigurationRepository;
        private readonly IWindyMapConfigurationRepository _windyMapConfigurationRepository;
        private readonly IMapper _mapper;

        public VFRMapConfigurationService(IAircraftLiveTrackerMapConfigurationRepository
            aircraftLiveTrackerMapConfigurationRepository, IMapper mapper,
            IVFRMapConfigurationRepository vFRMapConfigurationRepository,
            IWindyMapConfigurationRepository windyMapConfigurationRepository)
        {
            _vFRMapConfigurationRepository = vFRMapConfigurationRepository;
            _windyMapConfigurationRepository = windyMapConfigurationRepository;
            _aircraftLiveTrackerMapConfigurationRepository = aircraftLiveTrackerMapConfigurationRepository;
            _mapper = mapper;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                VFRMapConfigurationVM radarMapConfigurationVM = new();
                VFRMapConfiguration data = _vFRMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

                if (data is not null)
                {
                    radarMapConfigurationVM = _mapper.Map<VFRMapConfigurationVM>(data);
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

        public CurrentResponse SetDefault(VFRMapConfigurationVM vFRMapConfigurationVM)
        {
            try
            {
                VFRMapConfiguration radarMapConfiguration = _mapper.Map<VFRMapConfiguration>(vFRMapConfigurationVM);
                _vFRMapConfigurationRepository.SetDefault(radarMapConfiguration);

                if (vFRMapConfigurationVM.IsApplyToAll)
                {
                    _aircraftLiveTrackerMapConfigurationRepository.SetDefault(radarMapConfiguration.UserId, radarMapConfiguration.Height, radarMapConfiguration.Width);
                    _windyMapConfigurationRepository.SetDefault(radarMapConfiguration.UserId, radarMapConfiguration.Height, radarMapConfiguration.Width);
                }

                vFRMapConfigurationVM = _mapper.Map<VFRMapConfigurationVM>(radarMapConfiguration);

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
