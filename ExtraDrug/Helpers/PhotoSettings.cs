using ExtraDrug.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExtraDrug.Helpers;

public class PhotoSettings
{

    public int MaxBytes { get; set; }

    public ICollection<string> AcceptedFileTypes { get; set; } = new string[0];
    public bool IsValidSize(long size)
    {
        return size <= MaxBytes;
    }
    public bool IsValidMIME(string mime)
    {
        mime = mime.ToLower();
        return AcceptedFileTypes.Any(x => x.Equals(mime));
    }
    public RepoResult<IFormFile> ValidateFile(IFormFile file)
    {
        var errors = new List<string>();

        if (file.Length == 0)
            errors.Add("File Can't be empty");
        if (IsValidSize(file.Length))
            errors.Add("File size exeeded the limit");
        if (IsValidMIME(Path.GetExtension(file.FileName)))
            errors.Add("File Type Not Accepted");

        if (errors.Count > 0)
        {
            return new RepoResult<IFormFile> { Errors = errors  , Data = null , IsSucceeded = false};
        }
        return new RepoResult<IFormFile> { Data= file , Errors = null ,  IsSucceeded = true};
    }
}
