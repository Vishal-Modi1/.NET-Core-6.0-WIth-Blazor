﻿using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;
using DataModels.VM.Weather;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NOAARadarMapConfigurationController : BaseAPIController
    {
        private readonly INOAARadarMapConfigurationService _nOAARadarMapConfigurationService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public NOAARadarMapConfigurationController(INOAARadarMapConfigurationService nOAARadarMapConfigurationService, IHttpContextAccessor httpContextAccessor)
        {
            _nOAARadarMapConfigurationService = nOAARadarMapConfigurationService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(NOAARadarMapConfigurationVM nOAARadarMapConfigurationVM)
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            nOAARadarMapConfigurationVM.UserId = Convert.ToInt64(loggedInUser);
            CurrentResponse response = _nOAARadarMapConfigurationService.SetDefault(nOAARadarMapConfigurationVM);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _nOAARadarMapConfigurationService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}