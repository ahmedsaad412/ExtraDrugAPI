namespace ExtraDrug.Core.Models;

public class AuthResult:RepoResult<ApplicationUser>
{
    public string? JwtToken { get; set; }
    public DateTime? ExpiresOn { get; set; }
    public ICollection<string>? UserRoles { get; set; }
}
