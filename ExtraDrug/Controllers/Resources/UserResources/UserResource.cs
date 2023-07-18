using ExtraDrug.Controllers.Resources.DrugRequestResources;
using ExtraDrug.Controllers.Resources.UserDrugResources;
using ExtraDrug.Core.Models;

namespace ExtraDrug.Controllers.Resources.UserResources;

public class UserResource
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string? Username { get; set; } = "";

    public string? Email { get; set; } = "";

    public string? PhoneNumber { get; set; } = "";

    public string? Photo { get; set; }

    public ICollection<string> Roles { get; set; } = new List<string>();

    public ICollection<UserDrugResource> Drugs { get; set; } = new List<UserDrugResource>();


    public static UserResource MapToResource(ApplicationUser user)
    {
        return new UserResource()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Photo = user.PhotoAPIPath is null ? "/images/avatar_user_img.jpg" : user.PhotoAPIPath,
            Roles = user.Roles,
            Drugs = user.UserDrugs.Select(UserDrugResource.MapToResource).ToList(),            
        };
    }

}
