using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Web.UI.Controllers
{
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        public IWebHostEnvironment HostingEnvironment { get; set; }

        public FileUploadController(IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Route("api/fileupload/test")]
        public async Task<IActionResult> Test()
        {
            return Ok("test");
        }

        [Route("api/fileupload/save")]
        [HttpPost]
        public async Task<IActionResult> Save(IEnumerable<IFormFile> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                    // server Blazor app
                    //var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, fileName);
                    // client Blazor app
                    var physicalPath = Path.Combine(HostingEnvironment.ContentRootPath, fileName);

                    // implement security and validation here

                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // you do not have to do this, just an example of how you can
                    //Response.StatusCode = 222; //some custom status code, defaults to 200
                    //await Response.WriteAsync("some custom message"); // works with new EmptyResult()
                }
            }

            return new OkObjectResult("some custom message"); // new OkResult() sends an OK message without custom texts

            //return Content("response message"); // another way to return a custom message

        }

        // the same applies to the Delete action method, it is omitted here for brevity
    }
}
