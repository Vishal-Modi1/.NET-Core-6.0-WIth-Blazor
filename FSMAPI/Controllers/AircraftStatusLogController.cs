using DataModels.Constants;
using DataModels.VM.Aircraft.AircraftStatusLog;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftStatusLogController : BaseAPIController
    {
        private readonly IAircraftStatusLogService _aircraftStatusLogService;

        public AircraftStatusLogController(IAircraftStatusLogService aircraftStatusLogService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _aircraftStatusLogService = aircraftStatusLogService;
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(AircraftStatusLogDatatableParams datatableParam)
        {
            CurrentResponse response = _aircraftStatusLogService.List(datatableParam);

            return APIResponse(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftStatusLogVM aircraftStatusLog)
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);

            if (!string.IsNullOrEmpty(loggedInUser))
            {
                aircraftStatusLog.CreatedBy = Convert.ToInt64(loggedInUser);
            }

            CurrentResponse response = _aircraftStatusLogService.Create(aircraftStatusLog);
           
            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftStatusLogVM aircraftStatusLog)
        {
            aircraftStatusLog.UpdatedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftStatusLogService.Edit(aircraftStatusLog);

            return APIResponse(response);
        } 

        //[HttpGet]
        //[Route("getDetails")]
        //public IActionResult GetDetails(int id)
        //{
        //    CurrentResponse response = _aircraftStatusLogService.FindById(id);
        //    return APIResponse(response);
        //}

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftStatusLogService.Delete(id, deletedBy);

            return APIResponse(response);
        }
    }
}
