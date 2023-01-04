using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Reservation;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

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

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                userId = _jWTTokenGenerator.GetUserId();
            }

            CurrentResponse response = _reservationService.ListUpcomingFlightsByUserId(userId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByAircraftId")]
        public IActionResult ListUpcomingFlightsByAircraftId(long aircraftId)
        {
            CurrentResponse response = _reservationService.ListUpcomingFlightsByAircraftId(aircraftId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listUpcomingFlightsByCompanyId")]
        public IActionResult ListUpcomingFlightsByCompanyId(int companyId)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _reservationService.ListUpcomingFlightsByCompanyId(companyId);

            return APIResponse(response);
        }
    }
}
