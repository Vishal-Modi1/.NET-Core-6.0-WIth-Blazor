using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleDetailsController : ControllerBase
    {
        private readonly IModuleDetailsService _moduleDetailsService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public ModuleDetailsController(IModuleDetailsService moduleDetailsService, IHttpContextAccessor httpContextAccessor)
        {
            _moduleDetailsService = moduleDetailsService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _moduleDetailsService.ListDropDownValues();

            return Ok(response);
        }
    }
}
