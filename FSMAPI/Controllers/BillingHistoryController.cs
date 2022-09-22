using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.BillingHistory;
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
    public class BillingHistoryController : BaseAPIController
    {
        private readonly IBillingHistoryService _billingHistoryService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public BillingHistoryController(IBillingHistoryService billingHistoryService, IHttpContextAccessor httpContextAccessor)
        {
            _billingHistoryService = billingHistoryService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(BillingHistoryDatatableParams datatableParams)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != UserRole.SuperAdmin.ToString())
            {
                datatableParams.CompanyId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId));

                if (role.Replace(" ", "") != UserRole.Admin.ToString())
                {
                    datatableParams.UserId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
                }
            }

            CurrentResponse response = _billingHistoryService.List(datatableParams);

            return APIResponse(response);
        }
    }
}
