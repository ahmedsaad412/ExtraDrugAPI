namespace ExtraDrug.Core.Models;

public class RepoResult<T>
{
    public ICollection<string>? Errors { get; set; }
    public bool IsSucceeded { get; set; } = false;
    public T? Data { get; set; }
    
}
