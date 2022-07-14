using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftMakeController : ControllerBase
    {
        private readonly IAircraftMakeService _aircraftMakeService;

        public AircraftMakeController(IAircraftMakeService aircraftMakeService)
        {
            _aircraftMakeService = aircraftMakeService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftMake aircraftMake)
        {
            CurrentResponse response = _aircraftMakeService.Create(aircraftMake);

            return Ok(response);
        }

        [HttpGet]
        [Route("listall")]
        public IActionResult List()
        {
            CurrentResponse response = _aircraftMakeService.List();

            return Ok(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _aircraftMakeService.List(datatableParams);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _aircraftMakeService.Delete(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftMake aircraftMake)
        {
            CurrentResponse response = _aircraftMakeService.Edit(aircraftMake);
            return Ok(response);
        }
    }
}
