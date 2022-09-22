using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimezoneController : BaseAPIController
    {
        private readonly ITimezoneService _TimezoneService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public TimezoneController(ITimezoneService TimezoneService,
            IHttpContextAccessor httpContextAccessor)
        {
            _TimezoneService = TimezoneService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropdownValues()
        {
            CurrentResponse response = _TimezoneService.ListDropdownValues();

            return APIResponse(response);
        }
    }
}
