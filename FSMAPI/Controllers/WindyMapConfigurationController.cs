using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.Entities;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WindyMapConfigurationController : BaseAPIController
    {
        private readonly IWindyMapConfigurationService _WindyMapConfigurationService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public WindyMapConfigurationController(IWindyMapConfigurationService WindyMapConfigurationService, IHttpContextAccessor httpContextAccessor)
        {
            _WindyMapConfigurationService = WindyMapConfigurationService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(WindyMapConfiguration windyMapConfiguration)
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            windyMapConfiguration.UserId = Convert.ToInt64(loggedInUser);
            CurrentResponse response = _WindyMapConfigurationService.SetDefault(windyMapConfiguration);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _WindyMapConfigurationService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}
