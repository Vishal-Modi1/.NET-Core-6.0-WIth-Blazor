using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftModelController : ControllerBase
    {
        private readonly IAircraftModelService _aircraftModelService;

        public AircraftModelController(IAircraftModelService aircraftModelService)
        {
            _aircraftModelService = aircraftModelService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftModel aircraftModel)
        {
            CurrentResponse response = _aircraftModelService.Create(aircraftModel);

            return Ok(response);
        }

        [HttpGet]
        [Route("listall")]
        public IActionResult List()
        {
            CurrentResponse response = _aircraftModelService.List();

            return Ok(response);
        }

        
        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _aircraftModelService.List(datatableParams);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _aircraftModelService.Delete(id);

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftModel aircraftModel)
        {
            CurrentResponse response = _aircraftModelService.Edit(aircraftModel);
            return Ok(response);
        }
    }
}
