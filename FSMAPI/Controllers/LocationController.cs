using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;
using DataModels.VM.Location;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public LocationController(ILocationService locationService, IHttpContextAccessor httpContextAccessor)
        {
            _locationService = locationService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _locationService.List(datatableParams);

            return Ok(response);
        }

        //[HttpPost]
        //[Route("listall")]
        //public IActionResult ListAll()
        //{
        //    CurrentResponse response = _locationService.ListAll();

        //    return Ok(response);
        //}

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _locationService.ListDropDownValues();

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(LocationVM locationVM)
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);

            if (!string.IsNullOrEmpty(loggedInUser))
            {
                locationVM.CreatedBy = Convert.ToInt64(loggedInUser);
            }

            CurrentResponse response = _locationService.Create(locationVM);
            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(LocationVM locationVM)
        {
            locationVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _locationService.Edit(locationVM);
            return Ok(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _locationService.GetDetails(id);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _locationService.Delete(id, deletedBy);

            return Ok(response);
        }
    }
}
