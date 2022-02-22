using DataModels.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FSMAPI.Utilities
{
    public class FileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> UploadAsync(string directoryName, IFormCollection form, string fileName)
        {
            try
            {
                string uploadsPath = Path.Combine(_webHostEnvironment.ContentRootPath, UploadDirectory.RootDirectory);
                Directory.CreateDirectory(uploadsPath);

                Directory.CreateDirectory(uploadsPath + "\\" + directoryName);

                foreach (var formFile in form.Files)
                {
                    if (formFile.Length == 0)
                    {
                        continue;
                    }

                    string filePath = Path.Combine(uploadsPath, directoryName, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}
