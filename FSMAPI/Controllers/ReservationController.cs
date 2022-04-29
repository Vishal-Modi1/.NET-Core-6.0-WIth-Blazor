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
    public class ReservationController : ControllerBase
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

            return Ok(response);
        }

        [HttpGet]
        [Route("getfilters")]
        public IActionResult GetFilters(int roleId)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _reservationService.GetFiltersValue(roleId, CompanyId);

            return Ok(response);
        }
    }
}
