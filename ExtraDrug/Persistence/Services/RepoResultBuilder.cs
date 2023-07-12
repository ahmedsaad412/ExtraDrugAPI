using ExtraDrug.Core.Models;

namespace ExtraDrug.Persistence.Services;

public class RepoResultBuilder<T> where T: class
{
    public RepoResult<T> Success(T result)
    {
        return new RepoResult<T>()
        {
            Data = result,
            IsSucceeded = true,
            Errors = null
        };
    }
    public RepoResult<T> Failuer(ICollection<string> errors)
    {
        return new RepoResult<T>()
        {
            Data = null,
            IsSucceeded = false,
            Errors = errors
        };
    }
}
