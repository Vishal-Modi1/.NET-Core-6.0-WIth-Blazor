using DataModels.VM.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftStatusController : ControllerBase
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

            return Ok(response);
        }

        [HttpPost]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _aircraftStatusService.ListDropDownValues();

            return Ok(response);
        }

        [HttpGet]
        [Route("getbyid")]
        public IActionResult GetById(byte id)
        {
            CurrentResponse response = _aircraftStatusService.GetById(id);

            return Ok(response);
        }
    }
}
