using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleDetailsController : BaseAPIController
    {
        private readonly IModuleDetailsService _moduleDetailsService;

        public ModuleDetailsController(IModuleDetailsService moduleDetailsService, 
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _moduleDetailsService = moduleDetailsService;
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _moduleDetailsService.ListDropDownValues();

            return APIResponse(response);
        }
    }
}
