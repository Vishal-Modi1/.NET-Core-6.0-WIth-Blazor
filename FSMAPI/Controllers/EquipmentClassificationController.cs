using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class EquipmentClassificationController : BaseAPIController
    {
        private readonly IEquipmentClassificationService _equipmentClassificationService;

        public EquipmentClassificationController(IEquipmentClassificationService equipmentClassificationService)
        {
            _equipmentClassificationService = equipmentClassificationService;
        }
        
        [HttpGet]
        [Route("list")]
        public IActionResult List()
        {
            CurrentResponse response = _equipmentClassificationService.List();

            return APIResponse(response);
        }

    }
}
