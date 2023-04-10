using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.SubscriptionPlan;
using DataModels.Constants;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionPlanController : BaseAPIController
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly JWTTokenManager _jWTTokenManager;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService,
            IHttpContextAccessor httpContextAccessor)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(SubscriptionDataTableParams datatableParams)
        {
            CurrentResponse response = _subscriptionPlanService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(SubscriptionPlanVM subscriptionPlanVM)
        {
            subscriptionPlanVM.CreatedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _subscriptionPlanService.Create(subscriptionPlanVM);
           
            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(SubscriptionPlanVM subscriptionPlanVM)
        {
            subscriptionPlanVM.UpdatedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
         
            CurrentResponse response = _subscriptionPlanService.Edit(subscriptionPlanVM);
           
            return APIResponse(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _subscriptionPlanService.GetDetails(id);
            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _subscriptionPlanService.Delete(id, deletedBy);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("updatestatus")]
        public IActionResult UpdateStatus(long id, bool isActive)
        {
            CurrentResponse response = _subscriptionPlanService.UpdateActiveStatus(id, isActive);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("buyplan")]
        public IActionResult UpdateStatus(int id)
        {
            long userId = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _subscriptionPlanService.BuyPlan(id, userId);

            return APIResponse(response);
        }
    }
}
