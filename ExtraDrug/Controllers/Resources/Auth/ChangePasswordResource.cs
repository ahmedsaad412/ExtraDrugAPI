using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.Auth;

public class ChangePasswordResource
{
    [Required,MinLength(8)]
    public string? OldPassword { get; set; }
    [Required, MinLength(8)]
    public string? NewPassword { get; set; }
}
