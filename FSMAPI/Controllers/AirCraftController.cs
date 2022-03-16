using FSMAPI.Utilities;
using DataModels.Constants;
using DataModels.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using DataModels.VM.Aircraft;
using DataModels.VM.AircraftEquipment;
using System.Security.Claims;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _airCraftService;
        private readonly IAircraftEquipementTimeService _aircraftEquipementTimeService;
        private readonly IAircraftMakeService _aircraftMakeService;
        private readonly IAircraftModelService _aircraftModelService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly FileUploader _fileUploader;

        public AircraftController(IAircraftService airCraftService, IAircraftMakeService aircraftMakeService,
            IAircraftModelService aircraftModelService, IAircraftEquipementTimeService aircraftEquipementTimeService,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _airCraftService = airCraftService;
            _aircraftMakeService = aircraftMakeService;
            _aircraftModelService = aircraftModelService;
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

            CurrentResponse response = _airCraftService.GetFiltersValue(companyIdValue);

            return Ok(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(AircraftDatatableParams aircraftDatatableParams)
        {
            if (aircraftDatatableParams.CompanyId == 0)
            {
                string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
                aircraftDatatableParams.CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);
            }

            CurrentResponse response = _airCraftService.List(aircraftDatatableParams);

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftVM airCraftVM)
        {
            airCraftVM.CreatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _airCraftService.Create(airCraftVM);

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftVM airCraftVM)
        {
            airCraftVM.UpdatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _airCraftService.Edit(airCraftVM);

            return Ok(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _airCraftService.GetDetails(id, companyIdValue);
            return Ok(response);
        }

        [HttpGet]
        [Route("listAll")]
        public IActionResult ListAll()
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _airCraftService.ListAllByCompanyId(companyIdValue);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _airCraftService.Delete(id);

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

            string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{form["AircraftId"]}.jpeg";

            if(string.IsNullOrWhiteSpace(companyId))
            {
                companyId = form["CompanyId"];
            }

            bool isFileUploaded = await _fileUploader.UploadAsync(UploadDirectory.AircraftImage + "\\" + companyId , form, fileName);

            CurrentResponse response = new CurrentResponse();
            response.Data = "false";

            if (isFileUploaded)
            {
                response = _airCraftService.UpdateImageName(Convert.ToInt32(form["AircraftId"]), fileName);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("isaircraftexist")]
        public IActionResult IsAirCraftExist(int id, string tailNo)
        {
            CurrentResponse response = _airCraftService.IsAirCraftExist(id, tailNo);

            return Ok(response);
        }

        [HttpGet]
        [Route("listdropdownvalues")]
        public IActionResult ListAircraftDropdownValues()
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _airCraftService.ListAircraftDropdownValues(companyIdValue);
            return Ok(response);
        }

        #region Aircraft Make

        [HttpPost]
        [Route("createmake")]
        public IActionResult CreateMake(AircraftMake aircraftMake)
        {
            CurrentResponse response = _aircraftMakeService.Create(aircraftMake);

            return Ok(response);
        }

        [HttpGet]
        [Route("makelist")]
        public IActionResult MakeList()
        {
            CurrentResponse response = _aircraftMakeService.List();

            return Ok(response);
        }

        #endregion

        #region Aircraft Model

        [HttpPost]
        [Route("createmodel")]
        public IActionResult CreateModel(AircraftModel aircraftModel)
        {
            CurrentResponse response = _aircraftModelService.Create(aircraftModel);

            return Ok(response);
        }

        [HttpGet]
        [Route("modellist")]
        public IActionResult ModelList()
        {
            CurrentResponse response = _aircraftModelService.List();

            return Ok(response);
        }

        #endregion

        #region Aircraft Equipment
        [HttpPost]
        [Route("createaircraftequipment")]
        public IActionResult CreateAircraftEquipment(List<AircraftEquipmentTimeVM> aircraftEquipmentTimeVM)
        {
            long createdBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            if (aircraftEquipmentTimeVM.Count > 0)
            {
                _aircraftEquipementTimeService.DeleteAllEquipmentTimeByAirCraftId(aircraftEquipmentTimeVM.FirstOrDefault().AircraftId, createdBy);
            }

            CurrentResponse response = new CurrentResponse();

            aircraftEquipmentTimeVM.ForEach(item =>
            {
                item.CreatedBy = createdBy;
                response = _aircraftEquipementTimeService.Create(item);
            });

            return Ok(response);
        }
        #endregion
    }
}
