using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class WindyMapConfigurationService : BaseService, IWindyMapConfigurationService
    {
        private readonly IWindyMapConfigurationRepository _WindyMapConfigurationRepository;

        public WindyMapConfigurationService(IWindyMapConfigurationRepository WindyMapConfigurationRepository)
        {
            _WindyMapConfigurationRepository = WindyMapConfigurationRepository;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                WindyMapConfiguration data = _WindyMapConfigurationRepository.FindByCondition(p => p.UserId == userId);

                if(data == null)
                {
                    data = new WindyMapConfiguration();
                }

                CreateResponse(data, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(0, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(WindyMapConfiguration windyMapConfiguration)
        {
            try
            {
                _WindyMapConfigurationRepository.SetDefault(windyMapConfiguration);
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
