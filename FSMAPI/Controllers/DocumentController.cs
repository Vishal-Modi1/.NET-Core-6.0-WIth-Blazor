using DataModels.Constants;
using DataModels.VM.Document;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly FileUploader _fileUploader;
        private readonly IDocumentService _documentService;

        public DocumentController(IHttpContextAccessor httpContextAccessor,
            IDocumentService documentService,
            IWebHostEnvironment webHostEnvironment)
        {
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _fileUploader = new FileUploader(webHostEnvironment);
            _documentService = documentService;
        }

        [HttpGet]
        [Route("getfilters")]
        public IActionResult GetFilters()
        {
            CurrentResponse response = _documentService.GetFiltersValue();

            return Ok(response);
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFileAsync()
        {
            if (!Request.HasFormContentType)
            {
                return Ok(false);
            }

            IFormCollection form = Request.Form;

            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            string userId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);

            DocumentVM documentVM = _documentService.FindById(Guid.Parse(form.Files[0].Name));

            string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{form.Files[0].Name}.{documentVM.Type}";

            if (string.IsNullOrWhiteSpace(companyId))
            {
                companyId = form.Files[0].FileName;
            }

            string filePath = UploadDirectory.Document + "\\" + companyId + "\\" + userId;

            Directory.CreateDirectory(filePath);

            bool isFileUploaded = await _fileUploader.UploadAsync(filePath, form, fileName);

            CurrentResponse response = new CurrentResponse();
            response.Data = "false";

            if (isFileUploaded)
            {
                response = _documentService.UpdateDocumentName(Guid.Parse(form.Files[0].Name), fileName);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(DocumentVM documentVM)
        {
            documentVM.CreatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            if(documentVM.CompanyId == 0)
            {
                documentVM.CompanyId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId));
            }

            if (documentVM.UserId == 0)
            {
                documentVM.UserId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            }

            CurrentResponse response = _documentService.Create(documentVM);

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(DocumentVM documentVM)
        {
            documentVM.UpdatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            if (documentVM.CompanyId == 0)
            {
                documentVM.CompanyId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId));
            }

            if (documentVM.UserId == 0)
            {
                documentVM.UserId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            }

            CurrentResponse response = _documentService.Edit(documentVM);

            return Ok(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(Guid id)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _documentService.GetDetails(id, companyIdValue);
            return Ok(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DocumentDatatableParams datatableParams)
        {
            if (datatableParams.CompanyId == 0 && datatableParams.UserRole != DataModels.Enums.UserRole.SuperAdmin)
            {
                string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
                datatableParams.CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);
            }

            if (datatableParams.UserId == 0 && datatableParams.UserRole != DataModels.Enums.UserRole.SuperAdmin &&
                datatableParams.UserRole != DataModels.Enums.UserRole.Admin)
            {
                datatableParams.UserId = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            }

            CurrentResponse response = _documentService.List(datatableParams);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(Guid id)
        {
            CurrentResponse response = _documentService.Delete(id);

            return Ok(response);
        }

    }
}
