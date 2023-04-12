using Configuration;
using DataModels.Models;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using GlobalUtilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        private readonly APIErrorResponse _apiError;
        protected readonly JWTTokenManager _jWTTokenManager;
        protected readonly FileUploader _fileUploader;
        protected readonly ConfigurationSettings _configurationSettings;
        protected readonly RandomTextGenerator _randomTextGenerator;

        public BaseAPIController()
        {
            _apiError = new APIErrorResponse();
            _randomTextGenerator = new RandomTextGenerator();
            _configurationSettings = ConfigurationSettings.Instance;
        }

        public BaseAPIController(IHttpContextAccessor httpContextAccessor) : this()
        {
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
        }

        public BaseAPIController(IHttpContextAccessor httpContextAccessor,
             IWebHostEnvironment webHostEnvironment) : this()
        {
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
            _fileUploader = new FileUploader(webHostEnvironment);
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
