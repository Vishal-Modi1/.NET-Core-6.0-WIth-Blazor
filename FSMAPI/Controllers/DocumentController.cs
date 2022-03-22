﻿using DataModels.Constants;
using DataModels.VM.Document;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;
using DataModels.Entities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

            DocumentVM documentVM = new DocumentVM();

            documentVM.Id = Guid.Parse(form["Id"].ToString());
            documentVM.Name = form["Name"].ToString();
            documentVM.DisplayName = form["DisplayName"].ToString();
            documentVM.Type = form["Type"].ToString();
            documentVM.Size = Convert.ToInt64(form["Size"].ToString());
            documentVM.UserId = Convert.ToInt64(form["UserId"].ToString());
            documentVM.ModuleId = Convert.ToInt32(form["ModuleId"].ToString());
            documentVM.CompanyId = Convert.ToInt32(form["CompanyId"].ToString());
            documentVM.Tags = form["Tags"].ToString();

            if (!string.IsNullOrWhiteSpace(form["ExpirationDate"].ToString()))
            {
                documentVM.ExpirationDate = Convert.ToDateTime(form["ExpirationDate"].ToString());
            }

            CurrentResponse response = new CurrentResponse();

            if (documentVM.Id != Guid.Empty)
            {
                response = Edit(documentVM);
            }
            else
            {
                response = Create(documentVM);
            }

            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                return Ok(response);
            }

            Document document = (Document)response.Data;

            if (form.Files.Any(p => p.Length > 0))
            {
                string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{document.Id}.{documentVM.Type}";

                if (string.IsNullOrWhiteSpace(companyId))
                {
                    companyId = form["CompanyId"];
                }

                string filePath = UploadDirectory.Document + "\\" + companyId + "\\" + documentVM.UserId;

                Directory.CreateDirectory(filePath);

                bool isFileUploaded = await _fileUploader.UploadAsync(filePath, form, fileName);

                response.Data = "false";

                if (isFileUploaded)
                {
                    response = _documentService.UpdateDocumentName(document.Id, fileName);

                }
            }

            response = _documentService.FindByCondition(p => p.Id == document.Id);

            return Ok(response);
        }

        //[HttpPost]
        //[Route("create")]
        private CurrentResponse Create(DocumentVM documentVM)
        {
            documentVM.CreatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            if (documentVM.CompanyId == 0)
            {
                documentVM.CompanyId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId));
            }

            if (documentVM.UserId == 0)
            {
                documentVM.UserId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            }

            CurrentResponse response = _documentService.Create(documentVM);

            return response;
        }

        //[HttpPost]
        //[Route("edit")]
        private CurrentResponse Edit(DocumentVM documentVM)
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

            return response;
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
