using DataModels.Constants;
using DataModels.Entities;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftMakeController : BaseAPIController
    {
        private readonly IAircraftMakeService _aircraftMakeService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public AircraftMakeController(IAircraftMakeService aircraftMakeService, IHttpContextAccessor httpContextAccessor)
        {
            _aircraftMakeService = aircraftMakeService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftMake aircraftMake)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                aircraftMake.CompanyId = _jWTTokenGenerator.GetCompanyId();
            }

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
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                datatableParams.CompanyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _aircraftMakeService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues(int companyId)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _aircraftMakeService.ListDropDownValues(companyId);

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
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                aircraftMake.CompanyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _aircraftMakeService.Edit(aircraftMake);
            return APIResponse(response);
        }
    }
}
