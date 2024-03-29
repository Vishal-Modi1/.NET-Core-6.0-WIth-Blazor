﻿using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using DataModels.VM.Aircraft;
using DataModels.VM.AircraftEquipment;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftController : BaseAPIController
    {
        private readonly IAircraftService _aircraftService;
        private readonly IAircraftEquipementTimeService _aircraftEquipementTimeService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly FileUploader _fileUploader;

        public AircraftController(IAircraftService airCraftService,
             IAircraftEquipementTimeService aircraftEquipementTimeService,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _aircraftService = airCraftService;
            _aircraftEquipementTimeService = aircraftEquipementTimeService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _fileUploader = new FileUploader(webHostEnvironment);
        }

        [HttpGet]
        [Route("getfilters")]
        public IActionResult GetFilters()
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _aircraftService.GetFiltersValue(companyIdValue);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(AircraftDatatableParams aircraftDatatableParams)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                int companyId = _jWTTokenGenerator.GetCompanyId();
                if (companyId != aircraftDatatableParams.CompanyId && aircraftDatatableParams.CompanyId != 0)
                {
                    return APIResponse(UnAuthorizedResponse.Response());
                }

                aircraftDatatableParams.CompanyId = companyId;
            }

            CurrentResponse response = _aircraftService.List(aircraftDatatableParams);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftVM airCraftVM)
        {
            airCraftVM.CreatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            if (airCraftVM.OwnerId == 0)
            {
                airCraftVM.OwnerId = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            }

            CurrentResponse response = _aircraftService.Create(airCraftVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftVM airCraftVM)
        {
            airCraftVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            if (airCraftVM.OwnerId == 0)
            {
                airCraftVM.OwnerId = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            }

            CurrentResponse response = _aircraftService.Edit(airCraftVM);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(long id)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);
            CurrentResponse response = _aircraftService.GetDetails(id, companyIdValue);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listAll")]
        public IActionResult ListAll(int companyId)
        {
            if (companyId == 0)
            {
                string company = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
                companyId = company == "" ? 0 : Convert.ToInt32(company);
            }

            CurrentResponse response = _aircraftService.ListAllByCompanyId(companyId);
            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            long deletedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _aircraftService.Delete(id, deletedBy);

            return APIResponse(response);
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

            string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{form["AircraftId"]}.jpeg";

            if (string.IsNullOrWhiteSpace(companyId))
            {
                companyId = form["CompanyId"];
            }

            bool isFileUploaded = await _fileUploader.UploadAsync(UploadDirectories.AircraftImage + "\\" + companyId, form, fileName);

            CurrentResponse response = new CurrentResponse();
            response.Data = "false";

            if (isFileUploaded)
            {
                response = _aircraftService.UpdateImageName(Convert.ToInt64(form["AircraftId"]), fileName);
            }

            return APIResponse(response);
        }

        [HttpGet]
        [Route("isAircraftExist")]
        public IActionResult IsAircraftExist(long id, string tailNo)
        {
            CurrentResponse response = _aircraftService.IsAircraftExist(id, tailNo);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listDropdownValues")]
        public IActionResult ListAircraftDropdownValues()
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _aircraftService.ListAircraftDropdownValues(companyIdValue);
            return APIResponse(response);
        }


        [HttpGet]
        [Route("listDropdownValuesByCompanyId")]
        public IActionResult ListAircraftDropdownValuesByCompanyId(int companyId)
        {
            if (companyId == 0)
            {
                string companyIdValue = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
                companyId = companyIdValue == "" ? 0 : Convert.ToInt32(companyIdValue);
            }

            CurrentResponse response = _aircraftService.ListAircraftDropdownValues(companyId);
            return APIResponse(response);
        }

        [HttpGet]
        [Route("updatestatus")]
        public IActionResult UpdateStatus(long id, byte statusId)
        {
            CurrentResponse response = _aircraftService.UpdateStatus(id, statusId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("lockAircraft")]
        public IActionResult LockAircraft(long id, bool isLock)
        {
            CurrentResponse response = _aircraftService.LockAircraft(id, isLock);

            return APIResponse(response);
        }

        #region Aircraft Equipment

        [HttpPost]
        [Route("createaircraftequipment")]
        public IActionResult CreateAircraftEquipment(List<AircraftEquipmentTimeCreateVM> aircraftEquipmentTimeVM)
        {
            long createdBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            if (aircraftEquipmentTimeVM.Count > 0)
            {
                _aircraftEquipementTimeService.DeleteAllEquipmentTimeByAircraftId(aircraftEquipmentTimeVM.FirstOrDefault().AircraftId, createdBy);
            }

            CurrentResponse response = new CurrentResponse();

            aircraftEquipmentTimeVM.ForEach(item =>
            {
                item.CreatedBy = createdBy;
                response = _aircraftEquipementTimeService.Create(item);
            });

            return APIResponse(response);
        }
        #endregion
    }
}
