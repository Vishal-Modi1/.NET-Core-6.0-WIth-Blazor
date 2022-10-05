using DataModels.Models;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        private readonly APIErrorResponse _aPIError;
        public BaseAPIController()
        {
            _aPIError = new APIErrorResponse();
        }

        [HttpGet]
        [Route("response")]
        public IActionResult APIResponse(CurrentResponse response)
        {
            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                _aPIError.Message = response.Message;
                return StatusCode((int)response.Status, JsonConvert.SerializeObject(_aPIError));
            }

            return Ok(response);
        }
    }
}
