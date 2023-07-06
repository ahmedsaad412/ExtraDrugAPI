using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.Auth;

public class AddRoleToUserResource
{
    [Required ]
    public string? UserId { get; set; }
    [Required]
    public string? Role { get; set; }
}
