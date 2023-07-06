namespace ExtraDrug.Controllers.Resources.Auth;

public class AuthResource
{
    public string? Username { get; set; } 
    public string? Email { get; set; } 
    public ICollection<string>? Roles { get; set; }
    public string? Token { get; set; } 
    public DateTime? ExpiresOn { get; set; }

}
