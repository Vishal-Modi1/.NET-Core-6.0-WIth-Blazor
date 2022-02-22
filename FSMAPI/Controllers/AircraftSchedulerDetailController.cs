using DataModels.Constants;
using DataModels.VM;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Authorization;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftSchedulerDetailController : ControllerBase
    {
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly IAircraftScheduleDetailService _aircraftScheduleDetailService;

        public AircraftSchedulerDetailController(IAircraftScheduleDetailService aircraftScheduleDetailService, IHttpContextAccessor httpContextAccessor)
        {
            _aircraftScheduleDetailService = aircraftScheduleDetailService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("isaircraftalreadycheckout")]
        public IActionResult IsAircraftAlreadyCheckOut(long aircraftId)
        {
            CurrentResponse response = _aircraftScheduleDetailService.IsAircraftAlreadyCheckOut(aircraftId);

            return Ok(response);
        }

        [HttpGet]
        [Route("checkout")]
        public IActionResult Checkout(long scheduleId)
        {
            AircraftSchedulerDetailsVM aircraftScheduleDetailVM = new AircraftSchedulerDetailsVM();
            aircraftScheduleDetailVM.AircraftScheduleId = scheduleId;

            aircraftScheduleDetailVM.CheckOutBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleDetailService.CheckOut(aircraftScheduleDetailVM);

            return Ok(response);
        }

        [HttpGet]
        [Route("uncheckout")]
        public IActionResult UnCheckout(long scheduleId)
        {
            CurrentResponse response = _aircraftScheduleDetailService.UnCheckOut(scheduleId);

            return Ok(response);
        }

        [HttpPost]
        [Route("checkin")]
        public IActionResult CheckIn(List<AircraftEquipmentTimeVM> aircraftEquipmentsTimeList)
        {
            long checkInBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleDetailService.CheckIn(aircraftEquipmentsTimeList, checkInBy);

            return Ok(response);
        }
    }
}
