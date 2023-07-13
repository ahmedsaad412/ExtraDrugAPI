
using ExtraDrug.Core.Interfaces;

namespace ExtraDrug.Persistence.Services;


    public class FileService : IFileService
    {
        public async Task<string> AddStaticFile(IFormFile file, string UploadFolderPath)
        {
            if (!Directory.Exists(UploadFolderPath))
            {
                Directory.CreateDirectory(UploadFolderPath);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(UploadFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }


        public bool RemoveFile(string SystemFilePath)
        {
            if (File.Exists(SystemFilePath))
            {
                File.Delete(SystemFilePath);
                return true;
            }
            else
                return false;
        }
    }

