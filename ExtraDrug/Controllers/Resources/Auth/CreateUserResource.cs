using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.User;

public class CreateUserResource
{
    [Required,StringLength(100)]
    public string FirstName { get; set; } = "";
    
    [Required,StringLength(100)]
    public string LastName { get; set; } = "";
    
    [Required,StringLength(50)]
    public string Username { get; set; } = "";
    
    [Required, StringLength(128),EmailAddress]
    public string Email { get; set; } = "";
    
    [Required, StringLength(256)]
    public string Password { get; set; } = "";

    [Required , Phone]
    public string PhoneNumber { get; set; } = "";


    public ApplicationUser MapToModel()
    {
        return new ApplicationUser(){ 
        
            FirstName = FirstName, 
            LastName = LastName,
            Password = Password,
            Email = Email,
            UserName = Username,
            PhoneNumber = PhoneNumber,
        };
    }

}
