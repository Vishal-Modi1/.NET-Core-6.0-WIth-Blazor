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
        private readonly IWindyMapConfigurationRepository _WindyMapConfigurationRepository;
        private readonly IMapper _mapper;

        public WindyMapConfigurationService(IWindyMapConfigurationRepository 
            WindyMapConfigurationRepository, IMapper mapper)
        {
            _WindyMapConfigurationRepository = WindyMapConfigurationRepository;
            _mapper = mapper;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                WindyMapConfigurationVM windyMapConfigurationVM = new();
                WindyMapConfiguration data = _WindyMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

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
                _WindyMapConfigurationRepository.SetDefault(windyMapConfiguration);
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
