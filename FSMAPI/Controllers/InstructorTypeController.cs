using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.InstructorType;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Authorization;
using DataModels.Constants;
using FSMAPI.Utilities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InstructorTypeController : BaseAPIController
    {
        private readonly IInstructorTypeService _instructorTypeService;
        private readonly JWTTokenManager _jWTTokenManager;

        public InstructorTypeController(IInstructorTypeService instructorTypeService, IHttpContextAccessor httpContextAccessor)
        {
            _instructorTypeService = instructorTypeService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _instructorTypeService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(InstructorTypeVM instructorTypeVM)
        {
            CurrentResponse response = _instructorTypeService.Create(instructorTypeVM);
            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(InstructorTypeVM instructorTypeVM)
        {
            CurrentResponse response = _instructorTypeService.Edit(instructorTypeVM);
            return APIResponse(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _instructorTypeService.GetDetails(id);
            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _instructorTypeService.Delete(id, deletedBy);

            return APIResponse(response);
        }
    }
}
