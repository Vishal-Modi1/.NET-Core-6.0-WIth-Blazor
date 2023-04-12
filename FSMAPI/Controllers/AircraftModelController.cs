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
    public class AircraftModelController : BaseAPIController
    {
        private readonly IAircraftModelService _aircraftModelService;

        public AircraftModelController(IAircraftModelService aircraftModelService, 
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _aircraftModelService = aircraftModelService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftModel aircraftModel)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                aircraftModel.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _aircraftModelService.Create(aircraftModel);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listall")]
        public IActionResult List()
        {
            CurrentResponse response = _aircraftModelService.List();

            return APIResponse(response);
        }

        
        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _aircraftModelService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues(int companyId)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _aircraftModelService.ListDropDownValues(companyId);

            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _aircraftModelService.Delete(id);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftModel aircraftModel)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                aircraftModel.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _aircraftModelService.Edit(aircraftModel);
            return APIResponse(response);
        }
    }
}
