using DataModels.VM.Common;
using DataModels.VM.Document.DocumentDirectory;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;


namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentDirectoryController : BaseAPIController
    {
        private readonly IDocumentDirectoryService _documentDirectoryService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public DocumentDirectoryController(IDocumentDirectoryService documentDirectoryService,
           IHttpContextAccessor httpContextAccessor)
        {
            _documentDirectoryService = documentDirectoryService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(DocumentDirectoryVM documentDirectoryVM)
        {
            documentDirectoryVM.CreatedBy = _jWTTokenGenerator.GetUserId();
            documentDirectoryVM.CompanyId = _jWTTokenGenerator.GetCompanyId();

            CurrentResponse response = _documentDirectoryService.Create(documentDirectoryVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(DocumentDirectoryVM documentDirectoryVM)
        {
            documentDirectoryVM.UpdatedBy = _jWTTokenGenerator.GetUserId();
            documentDirectoryVM.CompanyId = _jWTTokenGenerator.GetCompanyId();

            CurrentResponse response = _documentDirectoryService.Edit(documentDirectoryVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            CurrentResponse response = _documentDirectoryService.Delete(id, _jWTTokenGenerator.GetUserId());

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listWithCountByComapnyId")]
        public IActionResult ListWithCountByComapnyId()
        {
            CurrentResponse response = _documentDirectoryService.ListWithCountByComapnyId(_jWTTokenGenerator.GetCompanyId());

            return APIResponse(response);
        }

        [HttpPost]
        [Route("fingById")]
        public IActionResult FindById(long id)
        {
            CurrentResponse response = _documentDirectoryService.FindById(id);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listDropDownValuesByCompanyId")]
        public IActionResult ListDropDownValuesByCompanyId()
        {
            CurrentResponse response = _documentDirectoryService.ListDropDownValuesByCompanyId(_jWTTokenGenerator.GetCompanyId());

            return APIResponse(response);
        }

    }
}
