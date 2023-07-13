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
        return new RepoResult<IFormFile> { Data= file };
    }
}
