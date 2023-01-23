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
    public class LogBookController : BaseAPIController
    {
        private readonly ILogBookService _logBookService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly FileUploader _fileUploader;

        public LogBookController(ILogBookService logBookService, IHttpContextAccessor httpContextAccessor,
             IWebHostEnvironment webHostEnvironment)
        {
            _logBookService = logBookService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _fileUploader = new FileUploader(webHostEnvironment);
        }

        //[HttpGet]
        //[Route("getfilters")]
        //public IActionResult GetFilters()
        //{
        //    string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
        //    int CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);

        //    CurrentResponse response = _logBookService.GetFiltersValue();

        //    return APIResponse(response);
        //}

        //[HttpPost]
        //[Route("list")]
        //public IActionResult List(CompanyDatatableParams datatableParams)
        //{
        //    string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);

        //    if (!string.IsNullOrWhiteSpace(companyId))
        //    {
        //        datatableParams.CompanyId = Convert.ToInt32(companyId);
        //    }

        //    CurrentResponse response = _logBookService.List(datatableParams);

        //    return APIResponse(response);
        //}

        //[HttpPost]
        //[Route("listall")]
        //public IActionResult ListAll()
        //{
        //    CurrentResponse response = _logBookService.ListAll();

        //    return APIResponse(response);
        //}

        //[HttpGet]
        //[Route("listdropdownvalues")]
        //public IActionResult ListDropDownValues()
        //{
        //    CurrentResponse response = _logBookService.ListDropDownValues();

        //    return APIResponse(response);
        //}

        [HttpGet]
        [Route("listInstrumentApproachesDropdownValues")]
        public IActionResult ListCompanyServicesDropDownValues()
        {
            CurrentResponse response = _logBookService.ListInstrumentApproachesDropdownValues();

            return APIResponse(response);
        }

        //[HttpPost]
        //[Route("create")]
        //public IActionResult Create(CompanyVM companyVM)
        //{
        //    string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);

        //    if (!string.IsNullOrEmpty(loggedInUser))
        //    {
        //        companyVM.CreatedBy = Convert.ToInt64(loggedInUser);
        //    }

        //    CurrentResponse response = _logBookService.Create(companyVM);
        //    return APIResponse(response);
        //}

        //[HttpPost]
        //[Route("edit")]
        //public IActionResult Edit(CompanyVM companyVM)
        //{
        //    companyVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

        //    CurrentResponse response = _logBookService.Edit(companyVM);
        //    return APIResponse(response);
        //}

        //[HttpGet]
        //[Route("getDetails")]
        //public IActionResult GetDetails(int id)
        //{
        //    CurrentResponse response = _logBookService.FindById(id);
        //    return APIResponse(response);
        //}

        //[HttpDelete]
        //[Route("delete")]
        //public IActionResult Delete(int id)
        //{
        //    long deletedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

        //    CurrentResponse response = _logBookService.Delete(id, deletedBy);

        //    return APIResponse(response);
        //}

        //[HttpPost]
        //[Route("uploadlogo")]
        //public async Task<IActionResult> UploadLogoAsync()
        //{
        //    if (!Request.HasFormContentType)
        //    {
        //        return Ok(false);
        //    }

        //    string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
        //    IFormCollection form = Request.Form;

        //    string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{form["CompanyId"]}.jpeg";

        //    if (string.IsNullOrWhiteSpace(companyId))
        //    {
        //        companyId = form["CompanyId"];
        //    }

        //    bool isFileUploaded = await _fileUploader.UploadAsync(UploadDirectories.CompanyLogo, form, fileName);

        //    CurrentResponse response = new CurrentResponse();
        //    response.Data = "";

        //    if (isFileUploaded)
        //    {
        //        response = _logBookService.UpdateImageName(Convert.ToInt32(form["CompanyId"]), fileName);
        //    }

        //    return APIResponse(response);
        //}

        //[HttpPut]
        //[Route("updatecreatedby")]
        //public IActionResult UpdateCreatedBy(int id, long createdBy)
        //{
        //    CurrentResponse response = _logBookService.UpdateCreatedBy(id, createdBy);
            
        //    return APIResponse(response);
        //}

        //[HttpPut]
        //[Route("iscompanyexist")]
        //public IActionResult IsCompanyExist(int id, string name)
        //{
        //    CurrentResponse response = _logBookService.IsCompanyExist(id, name);

        //    return APIResponse(response);
        //}

        //[HttpGet]
        //[Route("listDropdownValuesbyUserId")]
        //public IActionResult ListDropDownValuesByUserId(long userId)
        //{
        //    CurrentResponse response = _logBookService.ListDropDownValuesByUserId(userId);

        //    return APIResponse(response);
        //}

        //[HttpGet]
        //[Route("isDisplayPropeller")]
        //public IActionResult IsDisplayPropeller(int id)
        //{
        //    CurrentResponse response = _logBookService.IsDisplayPropeller(id);

        //    return APIResponse(response);
        //}

        //[HttpGet]
        //[Route("setPropellerConfiguration")]
        //public IActionResult SetPropellerConfiguration(bool value, int companyId = 0)
        //{
        //    string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

        //    if (role.Replace(" ", "") == DataModels.Enums.UserRole.SuperAdmin.ToString())
        //    {
        //        CurrentResponse response = _logBookService.SetPropellerConfiguration(companyId, value);

        //        return APIResponse(response);
        //    }
        //    else
        //    {
        //        string companyIdValue = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
        //        CurrentResponse response = _logBookService.SetPropellerConfiguration(Convert.ToInt32(companyIdValue), value);

        //        return APIResponse(response);
        //    }
        //}
    }
}
