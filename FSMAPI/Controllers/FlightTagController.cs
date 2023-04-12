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

        public FlightTagController(IFlightTagService flightTagService,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
            : base(httpContextAccessor, webHostEnvironment)
        {
            _flightTagService = flightTagService;
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
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _flightTagService.ListDropDownValues(companyId);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(FlightTagVM flightTagVM)
        {
            flightTagVM.CreatedBy = _jWTTokenManager.GetUserId();

            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                flightTagVM.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _flightTagService.Create(flightTagVM);

            return APIResponse(response);
        }
    }
}
