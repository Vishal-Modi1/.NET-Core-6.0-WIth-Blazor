using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Company.Settings;
using DataModels.VM.Document;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Data;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentTagController : BaseAPIController
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

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropdownValues()
        {
            CurrentResponse response = _documentTagService.ListDropDownValues();

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(DocumentTagVM documentTagVM)
        {
            documentTagVM.CreatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                documentTagVM.CompanyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _documentTagService.Create(documentTagVM);

            return APIResponse(response);
        }
    }
}
