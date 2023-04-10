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
        private readonly APIErrorResponse _apiError;
        public BaseAPIController()
        {
            _apiError = new APIErrorResponse();
        }

        [HttpGet]
        [Route("response")]
        protected IActionResult APIResponse(CurrentResponse response)
        {
            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                _apiError.Message = response.Message;
                return StatusCode((int)response.Status, JsonConvert.SerializeObject(_apiError));
            }

            return Ok(response);
        }
    }
}
