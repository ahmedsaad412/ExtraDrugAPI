using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.UserDrugResources;

public class EditUserResource
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(100)]
    public string LastName { get; set; } = "";

    [Required, StringLength(50)]
    public string Username { get; set; } = "";

    [Required, Phone]
    public string PhoneNumber { get; set; } = "";

    public ApplicationUser MapToModel()
    {
        return new ApplicationUser()
        {
            FirstName = FirstName,
            LastName = LastName,
            PhoneNumber = PhoneNumber,
            UserName = Username
        };
    }
}
