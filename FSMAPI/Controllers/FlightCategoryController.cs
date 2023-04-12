using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using DataModels.VM.Scheduler;
using DataModels.Constants;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightCategoryController : BaseAPIController
    {
        private readonly IFlightCategoryService _flightCategoryService;

        public FlightCategoryController(IFlightCategoryService flightCategoryService, 
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _flightCategoryService = flightCategoryService;
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _flightCategoryService.ListDropDownValues(_jWTTokenManager.GetCompanyId());

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listAll")]
        public IActionResult ListAll(int companyId)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _flightCategoryService.ListByCompanyId(companyId);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(FlightCategoryVM flightCategory)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                flightCategory.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _flightCategoryService.Create(flightCategory);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(FlightCategoryVM flightCategory)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                flightCategory.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _flightCategoryService.Edit(flightCategory);

            return APIResponse(response);
        }


        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _flightCategoryService.Delete(id);

            return APIResponse(response);
        }
    }
}
