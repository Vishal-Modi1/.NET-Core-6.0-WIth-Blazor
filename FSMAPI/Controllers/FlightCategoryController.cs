using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using DataModels.Entities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightCategoryController : BaseAPIController
    {
        private readonly IFlightCategoryService _flightCategoryService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public FlightCategoryController(IFlightCategoryService flightCategoryService, IHttpContextAccessor httpContextAccessor)
        {
            _flightCategoryService = flightCategoryService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _flightCategoryService.ListDropDownValues(_jWTTokenGenerator.GetCompanyId());

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listAll")]
        public IActionResult ListAll()
        {
            CurrentResponse response = _flightCategoryService.ListByCompanyId(_jWTTokenGenerator.GetCompanyId());

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(FlightCategory flightCategory)
        {
            flightCategory.CompanyId = _jWTTokenGenerator.GetCompanyId();
            CurrentResponse response = _flightCategoryService.Create(flightCategory);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(FlightCategory flightCategory)
        {
            flightCategory.CompanyId = _jWTTokenGenerator.GetCompanyId();
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
