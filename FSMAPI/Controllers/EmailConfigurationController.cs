using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailConfigurationController : BaseAPIController
    {
        private readonly IEmailConfigurationService _emailConfigurationService;

        public EmailConfigurationController(IEmailConfigurationService emailConfigurationService)
        {
            _emailConfigurationService = emailConfigurationService;
            
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(long id)
        {
            CurrentResponse response = _emailConfigurationService.GetDetails(id);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("getDetailsByEmailTypeAndCompanyId")]
        public IActionResult GetDetailsByEmailTypeAndCompanyId(int emailTypeId, int companyId)
        {
            CurrentResponse response = _emailConfigurationService.GetDetailsByEmailTypeAndCompanyId(emailTypeId, companyId);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(EmailConfigurationVM emailConfigurationVM)
        {
            CurrentResponse response = _emailConfigurationService.Create(emailConfigurationVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(EmailConfigurationVM emailConfigurationVM)
        {
            CurrentResponse response = _emailConfigurationService.Edit(emailConfigurationVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            CurrentResponse response = _emailConfigurationService.List(datatableParams);

            return APIResponse(response);
        }

    }
}
