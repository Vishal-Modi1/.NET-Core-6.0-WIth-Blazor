using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class ModuleDetailsService : BaseService, IModuleDetailsService
    {
        private readonly IModuleDetailsRepository _moduleDetailsRepository;

        public ModuleDetailsService(IModuleDetailsRepository moduleDetailsRepository)
        {
            _moduleDetailsRepository = moduleDetailsRepository;
        }

        public CurrentResponse ListDropDownValues()
        {
            try
            {
                List<DropDownValues> companiesList = _moduleDetailsRepository.ListDropDownValues();

                CreateResponse(companiesList, HttpStatusCode.OK, "");

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
