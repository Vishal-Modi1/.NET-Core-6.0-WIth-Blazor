using DataModels.VM.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillingHistoryController : ControllerBase
    {
        private readonly IBillingHistoryService _billingHistoryService;

        public BillingHistoryController(IBillingHistoryService billingHistoryService)
        {
            _billingHistoryService = billingHistoryService;
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _billingHistoryService.List(datatableParams);

            return Ok(response);
        }
    }
}
