using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscrepancyController : BaseAPIController
    {
        private readonly IDiscrepancyService _discrepancyService;
        private readonly IDiscrepancyHistoryService _discrepancyHistoryService;
        private readonly IDiscrepancyStatusService _discrepancyStatusService;
        private readonly JWTTokenManager _jWTTokenManager;

        public DiscrepancyController(IDiscrepancyService discrepancyService,
            IDiscrepancyStatusService discrepancyStatusService,
           IHttpContextAccessor httpContextAccessor, IDiscrepancyHistoryService discrepancyHistoryService)
        {
            _discrepancyService = discrepancyService;
            _discrepancyStatusService = discrepancyStatusService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
            _discrepancyHistoryService = discrepancyHistoryService;
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(long id)
        {
            CurrentResponse response = _discrepancyService.GetDetails(id);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(DiscrepancyVM discrepancyVM)
        {
            discrepancyVM.CreatedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));

            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);
            CurrentResponse response = _discrepancyService.Create(discrepancyVM, timezone);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(DiscrepancyVM discrepancyVM)
        {
            discrepancyVM.UpdatedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));

            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);
            CurrentResponse response = _discrepancyService.Edit(discrepancyVM, timezone);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DiscrepancyDatatableParams datatableParams)
        {
            CurrentResponse response = _discrepancyService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listStatusDropdownValues")]
        public IActionResult ListStatusDropdownValues()
        {
            CurrentResponse response = _discrepancyStatusService.ListDropDownValues();

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listDiscrepancyHistory")]
        public IActionResult ListDiscrepancyHistory(long discrepancyId)
        {
            CurrentResponse response = _discrepancyHistoryService.List(discrepancyId);

            return APIResponse(response);
        }
    }
}
