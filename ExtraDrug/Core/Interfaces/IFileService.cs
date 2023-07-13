namespace ExtraDrug.Core.Interfaces;
    public interface IFileService
    {
        Task<string> AddStaticFile(IFormFile file, string UploadFolderPath);
        bool RemoveFile(string SystemFilePath);


    }

