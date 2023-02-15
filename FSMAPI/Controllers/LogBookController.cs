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
            logBookVM.UpdatedBy = _jWTTokenGenerator.GetUserId();
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

                List<LogBookFlightPhoto> logBookFlightPhotosList = _logBookService.ListFlightPhotosByLogBookId(logBookId).Where(p=> string.IsNullOrWhiteSpace(p.Name)).OrderBy(p => p.Id).ToList();
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
                return APIResponse(new CurrentResponse() { Message = ex.ToString()});
            }
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(LogBookDatatableParams datatableParams)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                datatableParams.CompanyId = _jWTTokenGenerator.GetCompanyId();
                datatableParams.UserId = _jWTTokenGenerator.GetUserId();
            }

            CurrentResponse response = _logBookService.List(datatableParams);

            return APIResponse(response);
        }

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
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);
            CurrentResponse response = _logBookService.LogBookSummaries(_jWTTokenGenerator.GetUserId(), _jWTTokenGenerator.GetCompanyId(), role);
            
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

        [HttpDelete]
        [Route("deleteLogBookInstrumentApproach")]
        public IActionResult DeleteLogBookInstrumentApproach(long instrumentApproachId)
        {
            long deletedBy = _jWTTokenGenerator.GetUserId();
            CurrentResponse response = _logBookService.DeleteLogBookInstrumentApproach(instrumentApproachId, deletedBy);

            return APIResponse(response);
        }

        [HttpDelete]
        [Route("deleteLogBookCrewPassenger")]
        public IActionResult DeleteLogBookCrewPassenger(long logBookCrewPassengerId)
        {
            long deletedBy = _jWTTokenGenerator.GetUserId();
            CurrentResponse response = _logBookService.DeleteLogBookCrewPassenger(logBookCrewPassengerId, deletedBy);

            return APIResponse(response);
        }

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
    }
}
