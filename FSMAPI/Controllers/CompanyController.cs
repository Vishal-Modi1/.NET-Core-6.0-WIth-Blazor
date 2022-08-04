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

        [AllowAnonymous]
        [HttpGet]
        [Route("listcompanyservicesdropdownvalues")]
        public IActionResult ListCompanyServicesDropDownValues()
        {
            CurrentResponse response = _companyService.ListCompanyServiceDropDownValues();

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public IActionResult Create(CompanyVM companyVM)
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);

            if (!string.IsNullOrEmpty(loggedInUser))
            {
                companyVM.CreatedBy = Convert.ToInt64(loggedInUser);
            }

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

        [AllowAnonymous]
        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _companyService.FindById(id);
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

            bool isFileUploaded = await _fileUploader.UploadAsync(UploadDirectories.CompanyLogo, form, fileName);

            CurrentResponse response = new CurrentResponse();
            response.Data = "";

            if (isFileUploaded)
            {
                response = _companyService.UpdateImageName(Convert.ToInt32(form["CompanyId"]), fileName);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("updatecreatedby")]
        public IActionResult UpdateCreatedBy(int id, long createdBy)
        {
            CurrentResponse response = _companyService.UpdateCreatedBy(id, createdBy);
            
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("iscompanyexist")]
        public IActionResult IsCompanyExist(int id, string name)
        {
            CurrentResponse response = _companyService.IsCompanyExist(id, name);

            return Ok(response);
        }

        [HttpGet]
        [Route("listdropdownvaluesbyuserid")]
        public IActionResult ListDropDownValuesByUserId(long userId)
        {
            CurrentResponse response = _companyService.ListDropDownValuesByUserId(userId);

            return Ok(response);
        }
    }
}
