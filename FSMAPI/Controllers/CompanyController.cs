using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Company;
using DataModels.VM.Common;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(DatatableParams datatableParams)
        {
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

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CompanyVM companyVM)
        {
            CurrentResponse response = _companyService.Create(companyVM);
            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(CompanyVM companyVM)
        {
            CurrentResponse response = _companyService.Edit(companyVM);
            return Ok(response);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            CurrentResponse response = _companyService.GetDetails(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _companyService.Delete(id);

            return Ok(response);
        }
    }
}
