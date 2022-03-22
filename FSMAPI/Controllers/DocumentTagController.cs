using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Document;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentTagController : ControllerBase
    {
        private readonly IDocumentTagService _documentTagService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public DocumentTagController(IDocumentTagService documentTagService,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _documentTagService = documentTagService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult List()
        {
            CurrentResponse response = _documentTagService.List();

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(DocumentTagVM documentTagVM)
        {
            documentTagVM.CreatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _documentTagService.Create(documentTagVM);

            return Ok(response);
        }
    }
}
