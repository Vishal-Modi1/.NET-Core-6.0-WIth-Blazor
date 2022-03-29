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
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService,
            IHttpContextAccessor httpContextAccessor)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _subscriptionPlanService.List(datatableParams);

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(SubscriptionPlanVM subscriptionPlanVM)
        {
            subscriptionPlanVM.CreatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _subscriptionPlanService.Create(subscriptionPlanVM);
           
            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(SubscriptionPlanVM subscriptionPlanVM)
        {
            subscriptionPlanVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
         
            CurrentResponse response = _subscriptionPlanService.Edit(subscriptionPlanVM);
           
            return Ok(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _subscriptionPlanService.GetDetails(id);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _subscriptionPlanService.Delete(id, deletedBy);

            return Ok(response);
        }

        [HttpGet]
        [Route("updatestatus")]
        public IActionResult UpdateStatus(long id, bool isActive)
        {
            CurrentResponse response = _subscriptionPlanService.UpdateActiveStatus(id, isActive);

            return Ok(response);
        }
    }
}
