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

        public TimezoneController(ITimezoneService TimezoneService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _TimezoneService = TimezoneService;
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
