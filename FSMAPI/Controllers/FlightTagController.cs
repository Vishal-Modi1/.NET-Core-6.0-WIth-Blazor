using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Document;
using DataModels.VM.Scheduler;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightTagController : BaseAPIController
    {
        private readonly IFlightTagService _flightTagService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public FlightTagController(IFlightTagService flightTagService,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _flightTagService = flightTagService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult List()
        {
            CurrentResponse response = _flightTagService.List();

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropdownValues(int companyId)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _flightTagService.ListDropDownValues(companyId);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(FlightTagVM flightTagVM)
        {
            flightTagVM.CreatedBy = _jWTTokenGenerator.GetUserId();

            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                flightTagVM.CompanyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _flightTagService.Create(flightTagVM);

            return APIResponse(response);
        }
    }
}
