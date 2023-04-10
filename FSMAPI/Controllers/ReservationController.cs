using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Reservation;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interface;
using System.Reflection;
using GlobalUtilities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : BaseAPIController
    {
        private readonly IReservationService _reservationService;
        private readonly JWTTokenManager _jWTTokenManager;

        public ReservationController(IReservationService reservationService, IHttpContextAccessor httpContextAccessor)
        {
              _reservationService = reservationService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(ReservationDataTableParams datatableParams)
        {
            if (datatableParams.CompanyId == 0)
            {
                string companyId = _jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId);
                datatableParams.CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);
            }

            CurrentResponse response = _reservationService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("getfilters")]
        public IActionResult GetFilters(int roleId)
        {
            string companyId = _jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId);
            int CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _reservationService.GetFiltersValue(roleId, CompanyId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByUserId")]
        public IActionResult ListUpcomingFlightsByUserId(long userId)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);
            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);
            DateTime userTime = DateConverter.ToLocal(DateTime.UtcNow, timezone);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                userId = _jWTTokenManager.GetUserId();
            }

            CurrentResponse response = _reservationService.ListUpcomingFlightsByUserId(userId, userTime);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByAircraftId")]
        public IActionResult ListUpcomingFlightsByAircraftId(long aircraftId)
        {
            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);
            DateTime userTime = DateConverter.ToLocal(DateTime.UtcNow, timezone);

            CurrentResponse response = _reservationService.ListUpcomingFlightsByAircraftId(aircraftId, userTime);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByCompanyId")]
        public IActionResult ListUpcomingFlightsByCompanyId(int companyId)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);
            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);
            DateTime userTime =  DateConverter.ToLocal(DateTime.UtcNow, timezone);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _reservationService.ListUpcomingFlightsByCompanyId(companyId, userTime);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listReservationTypes")]
        public IActionResult ListReservationTypes()
        {
            BaseService baseService = new BaseService();
            CurrentResponse response = baseService.CreateResponse(_reservationService.ListReservationTypes(), System.Net.HttpStatusCode.OK, "");

            return APIResponse(response);
        }
    }
}
