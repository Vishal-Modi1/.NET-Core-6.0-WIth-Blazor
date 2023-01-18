using DataModels.VM.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AirTrafficControlCenterController : BaseAPIController
    {
        private readonly IAirTrafficControlCenterService _airTrafficControlCenterService;

        public AirTrafficControlCenterController(IAirTrafficControlCenterService airTrafficControlCenterService)
        {
            _airTrafficControlCenterService = airTrafficControlCenterService;
        }

        [HttpGet]
        [Route("listAll")]
        public IActionResult ListAll()
        {
            CurrentResponse response = _airTrafficControlCenterService.ListAll();

            return APIResponse(response);
        }
    }
}
