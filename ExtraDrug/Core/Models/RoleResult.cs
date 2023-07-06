namespace ExtraDrug.Core.Models;

public class RoleResult
{
    public ICollection<string>? Errors { get; set; }
    public bool IsSucceeded { get; set; } = false;
}
