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
        [Route("listByCompanyId")]
        public IActionResult List(int companyId)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

             long userId = _jWTTokenGenerator.GetUserId();
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _documentTagService.ListByCompanyId(companyId, userId, role);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listDropdownValues")]
        public IActionResult ListDropdownValuesByCompanyId(int companyId)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _documentTagService.ListDropDownValues(companyId);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(DocumentTagVM documentTagVM)
        {
            documentTagVM.CreatedBy = _jWTTokenGenerator.GetUserId();
           
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                documentTagVM.CompanyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _documentTagService.Create(documentTagVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(DocumentTagVM documentTagVM)
        {
            documentTagVM.UpdatedBy = _jWTTokenGenerator.GetUserId();

            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                documentTagVM.CompanyId = _jWTTokenGenerator.GetCompanyId();
            }

            CurrentResponse response = _documentTagService.Edit(documentTagVM);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("findById")]
        public IActionResult FindById(int id)
        {
            CurrentResponse response = _documentTagService.FindById(id);

            return APIResponse(response);
        }
    }
}
