using DataModels.Constants;

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
            return await UploadFiles(directoryName, form.Files[0], fileName);
        }

        public async Task<bool> UploadAsync(string directoryName, IFormFile file, string fileName)
        {
            return await UploadFiles(directoryName, file, fileName);
        }


        private async Task<bool> UploadFiles(string directoryName, IFormFile file, string fileName)
        {
            try
            {
                string uploadsPath = Path.Combine(_webHostEnvironment.ContentRootPath, UploadDirectories.RootDirectory);
                Directory.CreateDirectory(uploadsPath);

                Directory.CreateDirectory(uploadsPath + "\\" + directoryName);

                if (file.Length == 0)
                {
                    return false;
                }

                string filePath = Path.Combine(uploadsPath, directoryName, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
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
