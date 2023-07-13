using ExtraDrug.Core.Models;

namespace ExtraDrug.Controllers.Resources.UserDrugResources;

public class UserDrugPhotoResource
{
    public int Id { get; set; }
    public string APIPath { get; set; } = string.Empty;

    public static UserDrugPhotoResource MapToResource(UserDrugPhoto udp)
    {
        return new UserDrugPhotoResource()
        {
            Id = udp.Id,
            APIPath = udp.APIPath,
        };
    }
}
