using ExtraDrug.Core.Models;

namespace ExtraDrug.Controllers.Resources.Auth;

public class UserResource
{
    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string? Username { get; set; } = "";

    public string? Email { get; set; } = "";

    public string? PhoneNumber { get; set; } = "";
    public ICollection<string> Roles { get; set; } = new List<string>();

    public static UserResource MapToResource(ApplicationUser user)
    {
        return new UserResource()
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName, 
            Username = user.UserName , 
            PhoneNumber = user.PhoneNumber,
            Roles = user.Roles
        };
    }
    
}
