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
        private readonly IRadarMapConfigurationRepository _radarMapConfigurationRepository;
        private readonly IVFRMapConfigurationRepository _vFRMapConfigurationRepository;
        private readonly IWindyMapConfigurationRepository _windyMapConfigurationRepository;
        private readonly IMapper _mapper;

        public VFRMapConfigurationService(IAircraftLiveTrackerMapConfigurationRepository
            aircraftLiveTrackerMapConfigurationRepository, IMapper mapper,
            IVFRMapConfigurationRepository vFRMapConfigurationRepository,
            IWindyMapConfigurationRepository windyMapConfigurationRepository, IRadarMapConfigurationRepository radarMapConfigurationRepository)
        {
            _vFRMapConfigurationRepository = vFRMapConfigurationRepository;
            _radarMapConfigurationRepository = radarMapConfigurationRepository;
            _windyMapConfigurationRepository = windyMapConfigurationRepository;
            _aircraftLiveTrackerMapConfigurationRepository = aircraftLiveTrackerMapConfigurationRepository;
            _mapper = mapper;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                VFRMapConfigurationVM vFRMapConfigurationVM = new();
                VFRMapConfiguration data = _vFRMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

                if (data is not null)
                {
                    vFRMapConfigurationVM = _mapper.Map<VFRMapConfigurationVM>(data);
                }

                CreateResponse(vFRMapConfigurationVM, HttpStatusCode.OK, "");

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
                VFRMapConfiguration vFRMapConfiguration = _mapper.Map<VFRMapConfiguration>(vFRMapConfigurationVM);
                _vFRMapConfigurationRepository.SetDefault(vFRMapConfiguration);

                if (vFRMapConfigurationVM.IsApplyToAll)
                {
                    _aircraftLiveTrackerMapConfigurationRepository.SetDefault(vFRMapConfiguration.UserId, vFRMapConfiguration.Height, vFRMapConfiguration.Width);
                    _windyMapConfigurationRepository.SetDefault(vFRMapConfiguration.UserId, vFRMapConfiguration.Height, vFRMapConfiguration.Width);
                    _radarMapConfigurationRepository.SetDefault(vFRMapConfiguration.UserId, vFRMapConfiguration.Height, vFRMapConfiguration.Width);
                }

                vFRMapConfigurationVM = _mapper.Map<VFRMapConfigurationVM>(vFRMapConfiguration);

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
