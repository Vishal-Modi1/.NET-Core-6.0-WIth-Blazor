using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Company;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;
using DataModels.VM.LogBook;
using DataModels.Entities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

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

        [HttpPost]
        [Route("create")]
        public IActionResult Create(LogBookVM logBookVM)
        {
            logBookVM.CreatedBy = _jWTTokenGenerator.GetUserId();
            logBookVM.CompanyId = _jWTTokenGenerator.GetCompanyId();

            CurrentResponse response = _logBookService.Create(logBookVM);
            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(LogBookVM logBookVM)
        {
            logBookVM.CreatedBy = _jWTTokenGenerator.GetUserId();
            logBookVM.CompanyId = _jWTTokenGenerator.GetCompanyId();

            CurrentResponse response = _logBookService.Edit(logBookVM);
            return APIResponse(response);
        }

        [HttpPost]
        [Route("uploadFlightPhotos")]
        public async Task<IActionResult> UploadFlightPhotos()
        {
            try
            {
                if (!Request.HasFormContentType)
                {
                    return Ok(false);
                }

                IFormCollection form = Request.Form;

                string companyId = _jWTTokenGenerator.GetCompanyId().ToString();
                string userId = _jWTTokenGenerator.GetUserId().ToString();

                long logBookId = Convert.ToInt64(form["LogBookId"]);

                List<LogBookFlightPhoto> logBookFlightPhotosList = _logBookService.ListFlightPhotosByLogBookId(logBookId).OrderBy(p => p.Id).ToList();
                string filePath = UploadDirectories.LogbookFlightPhoto + "\\" + companyId + "\\" + userId + "\\" + logBookId;

                int i = 0;
                foreach (var item in form.Files)
                {
                    i++;
                    string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{form["LogBookId"]}_{i}.jpeg";
                    bool isFileUploaded = await _fileUploader.UploadAsync(filePath, item, fileName);

                    LogBookFlightPhoto logBookFlightPhoto = logBookFlightPhotosList.Skip(i - 1).Take(1).First();

                    logBookFlightPhoto.Name = fileName;
                }

                logBookFlightPhotosList.RemoveAll(p => string.IsNullOrWhiteSpace(p.Name));

                CurrentResponse response = new CurrentResponse();
                response.Data = "false";

                response = _logBookService.UpdateImagesName(logBookId, logBookFlightPhotosList);

                return APIResponse(response);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //[HttpPost]
        //[Route("edit")]
        //public IActionResult Edit(CompanyVM companyVM)
        //{
        //    companyVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

        //    CurrentResponse response = _logBookService.Edit(companyVM);
        //    return APIResponse(response);
        //}

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(long id)
        {
            CurrentResponse response = _logBookService.FindById(id);
            return APIResponse(response);
        }

        [HttpGet]
        [Route("logBookSummaries")]
        public IActionResult LogBookSummaries()
        {
            CurrentResponse response = _logBookService.LogBookSummaries(_jWTTokenGenerator.GetUserId(), _jWTTokenGenerator.GetCompanyId());
            return APIResponse(response);
        }

        [HttpDelete]
        [Route("deletePhoto")]
        public IActionResult DeletePhoto(long photoId)
        {
            long deletedBy = _jWTTokenGenerator.GetUserId();
            CurrentResponse response = _logBookService.DeletePhoto(photoId, deletedBy);

            return APIResponse(response);
        }


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

        [HttpGet]
        [Route("listPassengersRolesDropdownValues")]
        public IActionResult ListPassengersRolesDropdownValues()
        {
            CurrentResponse response = _logBookService.ListPassengersRolesDropdownValues();

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listPassengersDropdownValuesByCompanyId")]
        public IActionResult ListPassengersDropdownValuesByCompanyId()
        {
            CurrentResponse response = _logBookService.ListPassengersDropdownValuesByCompanyId(_jWTTokenGenerator.GetCompanyId());

            return APIResponse(response);
        }

        [HttpPost]
        [Route("createCrewPassenger")]  
        public IActionResult Create(CrewPassengerVM crewPassengerVM)
        {
            crewPassengerVM.CompanyId = _jWTTokenGenerator.GetCompanyId();
            CurrentResponse response = _logBookService.CreateCrewPassenger(crewPassengerVM);
            return APIResponse(response);
        }

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
