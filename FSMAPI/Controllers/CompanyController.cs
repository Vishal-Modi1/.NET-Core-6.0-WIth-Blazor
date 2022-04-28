using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Company;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly FileUploader _fileUploader;

        public CompanyController(ICompanyService companyService, IHttpContextAccessor httpContextAccessor,
             IWebHostEnvironment webHostEnvironment)
        {
            _companyService = companyService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _fileUploader = new FileUploader(webHostEnvironment);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);

            if (!string.IsNullOrWhiteSpace(companyId))
            {
                datatableParams.CompanyId = Convert.ToInt32(companyId);
            }

            CurrentResponse response = _companyService.List(datatableParams);

            return Ok(response);
        }

        [HttpPost]
        [Route("listall")]
        public IActionResult ListAll()
        {
            CurrentResponse response = _companyService.ListAll();

            return Ok(response);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListDropDownValues()
        {
            CurrentResponse response = _companyService.ListDropDownValues();

            return Ok(response);
        }

        [HttpGet]
        [Route("listcompanyservicesdropdownvalues")]
        public IActionResult ListCompanyServicesDropDownValues()
        {
            CurrentResponse response = _companyService.ListCompanyServiceDropDownValues();

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CompanyVM companyVM)
        {
            companyVM.CreatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _companyService.Create(companyVM);
            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(CompanyVM companyVM)
        {
            companyVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _companyService.Edit(companyVM);
            return Ok(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _companyService.GetDetails(id);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _companyService.Delete(id, deletedBy);

            return Ok(response);
        }

        [HttpPost]
        [Route("uploadlogo")]
        public async Task<IActionResult> UploadLogoAsync()
        {
            if (!Request.HasFormContentType)
            {
                return Ok(false);
            }

            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            IFormCollection form = Request.Form;

            string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{form["CompanyId"]}.jpeg";

            if (string.IsNullOrWhiteSpace(companyId))
            {
                companyId = form["CompanyId"];
            }

            bool isFileUploaded = await _fileUploader.UploadAsync(UploadDirectory.CompanyLogo, form, fileName);

            CurrentResponse response = new CurrentResponse();
            response.Data = "";

            if (isFileUploaded)
            {
                response = _companyService.UpdateImageName(Convert.ToInt32(form["CompanyId"]), fileName);
            }

            return Ok(response);
        }
    }
}
