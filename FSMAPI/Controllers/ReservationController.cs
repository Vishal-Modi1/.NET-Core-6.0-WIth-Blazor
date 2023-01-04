using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Reservation;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Utilities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : BaseAPIController
    {
        private readonly IReservationService _reservationService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public ReservationController(IReservationService reservationService, IHttpContextAccessor httpContextAccessor)
        {
              _reservationService = reservationService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(ReservationDataTableParams datatableParams)
        {
            if (datatableParams.CompanyId == 0)
            {
                string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
                datatableParams.CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);
            }

            CurrentResponse response = _reservationService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("getfilters")]
        public IActionResult GetFilters(int roleId)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _reservationService.GetFiltersValue(roleId, CompanyId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByUserId")]
        public IActionResult ListUpcomingFlightsByUserId(long userId)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            string timezone = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.TimeZone);
            DateTime userTime = DateConverter.ToLocal(DateTime.UtcNow, timezone);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                userId = _jWTTokenGenerator.GetUserId();
            }

            CurrentResponse response = _reservationService.ListUpcomingFlightsByUserId(userId, userTime);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByAircraftId")]
        public IActionResult ListUpcomingFlightsByAircraftId(long aircraftId)
        {
            string timezone = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.TimeZone);
            DateTime userTime = DateConverter.ToLocal(DateTime.UtcNow, timezone);

            CurrentResponse response = _reservationService.ListUpcomingFlightsByAircraftId(aircraftId, userTime);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByCompanyId")]
        public IActionResult ListUpcomingFlightsByCompanyId(int companyId)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            string timezone = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.TimeZone);
            DateTime userTime =  DateConverter.ToLocal(DateTime.UtcNow, timezone);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _reservationService.ListUpcomingFlightsByCompanyId(companyId, userTime);

            return APIResponse(response);
        }
    }
}
