using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.InstructorType;
using DataModels.VM.Common;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorTypeController : ControllerBase
    {
        private readonly IInstructorTypeService _instructorTypeService;

        public InstructorTypeController(IInstructorTypeService instructorTypeService)
        {
            _instructorTypeService = instructorTypeService;
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _instructorTypeService.List(datatableParams);

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(InstructorTypeVM instructorTypeVM)
        {
            CurrentResponse response = _instructorTypeService.Create(instructorTypeVM);
            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(InstructorTypeVM instructorTypeVM)
        {
            CurrentResponse response = _instructorTypeService.Edit(instructorTypeVM);
            return Ok(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _instructorTypeService.GetDetails(id);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _instructorTypeService.Delete(id);

            return Ok(response);
        }
    }
}
