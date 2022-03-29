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
    public class AircraftEquipmentController : Controller
    {
        private readonly IAirCraftEquipmentService _airCraftEquipmentService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public AircraftEquipmentController(IAirCraftEquipmentService airCraftEquipmentService, IHttpContextAccessor httpContextAccessor)
        {
            _airCraftEquipmentService = airCraftEquipmentService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(AircraftEquipmentsVM airCraftEquipmentsVM)
        {
            airCraftEquipmentsVM.CreatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue("Id"));
            CurrentResponse response = _airCraftEquipmentService.Create(airCraftEquipmentsVM);

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AircraftEquipmentsVM airCraftEquipmentsVM)
        {
            airCraftEquipmentsVM.UpdatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue("Id"));
            CurrentResponse response = _airCraftEquipmentService.Edit(airCraftEquipmentsVM);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId)); 
            
            CurrentResponse response = _airCraftEquipmentService.Delete(id, deletedBy);

            return Ok(response);
        }


       // With server side pagination
        [HttpPost]
        [Route("list")]
        public IActionResult List(AircraftEquipmentDatatableParams datatableParams)
        {
            CurrentResponse response = _airCraftEquipmentService.List(datatableParams);
            return Ok(response);
        }

        //[HttpGet]
        //[Route("list")]
        //public IActionResult List(int aircraftId)
        //{
        //    CurrentResponse response = _airCraftEquipmentService.List(aircraftId);
        //    return Ok(response);
        //}

        [HttpGet]
        [Route("fetchbyid")]
        public IActionResult fetchbyid(int id)
        {
            CurrentResponse response = _airCraftEquipmentService.Get(id);
            return Ok(response);
        }
    }
}
