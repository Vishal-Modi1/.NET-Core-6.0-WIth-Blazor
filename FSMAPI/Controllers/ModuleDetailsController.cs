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
        private readonly JWTTokenManager _jWTTokenManager;

        public ModuleDetailsController(IModuleDetailsService moduleDetailsService, IHttpContextAccessor httpContextAccessor)
        {
            _moduleDetailsService = moduleDetailsService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
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
