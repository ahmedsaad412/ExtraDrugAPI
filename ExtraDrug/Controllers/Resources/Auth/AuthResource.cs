using ExtraDrug.Core.Models;

namespace ExtraDrug.Controllers.Resources.Auth;

public class AuthResource
{
    public string? Username { get; set; } 
    public string? UserId { get; set; }
    public string? Email { get; set; } 
    public string? PhoneNumber { get; set; }
    public ICollection<string>? Roles { get; set; }
    public string? Token { get; set; } 
    public DateTime? ExpiresOn { get; set; }

    public static AuthResource MapToResource(AuthResult res)
    {
        return new AuthResource()
        {
            Username = res.Data.UserName,
            Email = res.Data.Email,
            Roles = res.UserRoles,
            UserId = res.Data.Id,
            Token = res.JwtToken,
            ExpiresOn = res.ExpiresOn,
            PhoneNumber = res.Data.PhoneNumber
        };
    }

}
