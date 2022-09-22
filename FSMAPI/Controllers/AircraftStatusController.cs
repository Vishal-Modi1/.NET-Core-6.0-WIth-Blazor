using DataModels.VM.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftStatusController : BaseAPIController
    {
        private readonly IAircraftStatusService _aircraftStatusService;

        public AircraftStatusController(IAircraftStatusService aircraftStatusService)
        {
            _aircraftStatusService = aircraftStatusService;
        }

        [HttpPost]
        [Route("listall")]
        public IActionResult ListAll()
        {
            CurrentResponse response = _aircraftStatusService.ListAll();

            return APIResponse(response);
        }

        [HttpPost]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _aircraftStatusService.ListDropDownValues();

            return APIResponse(response);
        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult GetById(byte id)
        {
            CurrentResponse response = _aircraftStatusService.GetById(id);

            return APIResponse(response);
        }
    }
}
