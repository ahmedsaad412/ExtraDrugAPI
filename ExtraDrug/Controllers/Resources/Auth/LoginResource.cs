using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.Auth;

public class LoginResource
{
    [Required,EmailAddress]

    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }

    public ApplicationUser MapToModel()
    {
        return new ApplicationUser()
        {   
            Password = Password,
            Email = Email,
        };
    }
}
