using DataModels.Constants;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscrepancyFileController : BaseAPIController
    {
        private readonly IDiscrepancyFileService _discrepancyFileService;
        private readonly IDiscrepancyService _discrepancyService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly FileUploader _fileUploader;

        public DiscrepancyFileController(IDiscrepancyFileService discrepancyFileService,
            IDiscrepancyService discrepancyService,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _discrepancyFileService = discrepancyFileService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _fileUploader = new FileUploader(webHostEnvironment);
            _discrepancyService = discrepancyService;
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

            DiscrepancyFileVM discrepancyFileVM = new DiscrepancyFileVM();

            discrepancyFileVM.Id = Convert.ToInt64(form["Id"].ToString());
            discrepancyFileVM.Name = form["Name"].ToString();
            discrepancyFileVM.DisplayName = form["DisplayName"].ToString();
            discrepancyFileVM.DiscrepancyId = Convert.ToInt64(form["DiscrepancyId"].ToString());

            CurrentResponse response = new CurrentResponse();

            if (discrepancyFileVM.Id > 0)
            {
                response = Edit(discrepancyFileVM);
            }
            else
            {
                response = Create(discrepancyFileVM);
            }

            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                return APIResponse(response);
            }

            DiscrepancyFile discrepancyFile = (DiscrepancyFile)response.Data;

            if (form.Files.Any(p => p.Length > 0))
            {
                Discrepancy discrepancy = _discrepancyService.FindByCondition(p => p.Id == discrepancyFile.DiscrepancyId);

                string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{discrepancyFile.Id}{Path.GetExtension(discrepancyFileVM.Name)}";
                string filePath = UploadDirectories.Discrepancy + "\\" + discrepancy.CompanyId + "\\" + discrepancy.AircraftId;

                Directory.CreateDirectory(filePath);

                bool isFileUploaded = await _fileUploader.UploadAsync(filePath, form, fileName);

                response.Data = "false";

                if (isFileUploaded)
                {
                    response = _discrepancyFileService.UpdateDocumentName(discrepancyFile.Id, fileName);
                }
            }

            response = _discrepancyFileService.FindByCondition(p => p.Id == discrepancyFile.Id);
            response.Message = "File uploaded successfully.";

            return APIResponse(response);
        }

        private CurrentResponse Create(DiscrepancyFileVM discrepancyFileVM)
        {
            discrepancyFileVM.CreatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _discrepancyFileService.Create(discrepancyFileVM);

            return response;
        }

        private CurrentResponse Edit(DiscrepancyFileVM discrepancyFileVM)
        {
            discrepancyFileVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _discrepancyFileService.Edit(discrepancyFileVM);

            return response;
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            long deletedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _discrepancyFileService.Delete(id, deletedBy);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult List(long discrepancyId)
        {
            CurrentResponse response = _discrepancyFileService.List(discrepancyId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(long id)
        {
            CurrentResponse response = _discrepancyFileService.GetDetails(id);

            return APIResponse(response);
        }
    }
}
