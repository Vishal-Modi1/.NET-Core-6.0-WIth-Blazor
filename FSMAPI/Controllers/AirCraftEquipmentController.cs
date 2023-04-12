using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Http;
using DataModels.VM.AircraftEquipment;
using DataModels.Constants;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftEquipmentController : BaseAPIController
    {
        private readonly IAircraftEquipmentService _airCraftEquipmentService;

        public AircraftEquipmentController(IAircraftEquipmentService aircraftEquipmentService, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _airCraftEquipmentService = aircraftEquipmentService;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftEquipmentsVM airCraftEquipmentsVM)
        {
            airCraftEquipmentsVM.CreatedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _airCraftEquipmentService.Create(airCraftEquipmentsVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftEquipmentsVM airCraftEquipmentsVM)
        {
            airCraftEquipmentsVM.UpdatedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _airCraftEquipmentService.Edit(airCraftEquipmentsVM);

            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId)); 
            
            CurrentResponse response = _airCraftEquipmentService.Delete(id, deletedBy);

            return APIResponse(response);
        }


       // With server side pagination
        [HttpPost]
        [Route("list")]
        public IActionResult List(AircraftEquipmentDatatableParams datatableParams)
        {
            CurrentResponse response = _airCraftEquipmentService.List(datatableParams);
            return APIResponse(response);
        }

        //[HttpGet]
        //[Route("list")]
        //public IActionResult List(int aircraftId)
        //{
        //    CurrentResponse response = _airCraftEquipmentService.List(aircraftId);
        //    return APIResponse(response);
        //}

        [HttpGet]
        [Route("fetchbyid")]
        public IActionResult fetchbyid(int id)
        {
            CurrentResponse response = _airCraftEquipmentService.Get(id);
            return APIResponse(response);
        }
    }
}
