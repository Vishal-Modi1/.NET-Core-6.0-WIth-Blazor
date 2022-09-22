using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftMakeController : BaseAPIController
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

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listall")]
        public IActionResult List()
        {
            CurrentResponse response = _aircraftMakeService.List();

            return APIResponse(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _aircraftMakeService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _aircraftMakeService.ListDropDownValues();

            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _aircraftMakeService.Delete(id);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftMake aircraftMake)
        {
            CurrentResponse response = _aircraftMakeService.Edit(aircraftMake);
            return APIResponse(response);
        }
    }
}
