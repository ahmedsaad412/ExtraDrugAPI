using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExtraDrug.Controllers.Resources.UserDrugResources;

public class UserDrugResource
{
    public DateTime ExpireDate { get; set; }

    public int Id { get; set; }
    public int Quantity { get; set; }
    public double CoordsLongitude { get; set; }
    public double CoordsLatitude { get; set; }
    public string UserId { get; set; }
    public  DateTime CreatedAt { get; set; }
    public DrugResource? Drug { get; set; }
    public ICollection<UserDrugPhotoResource> Photos { get; set; } = new List<UserDrugPhotoResource>();
    public static UserDrugResource MapToResource(UserDrug ud)
    {
        return new UserDrugResource()
        {
            Id = ud.Id,
            CoordsLatitude = ud.CoordsLatitude,
            CoordsLongitude = ud.CoordsLongitude,
            ExpireDate = ud.ExpireDate,
            Quantity = ud.Quantity,
            CreatedAt = ud.CreatedAt,
            Drug = ud.Drug is not null ? DrugResource.MapToResource(ud.Drug) : null,
            Photos = ud.Photos.Count > 0 ? 
                ud.Photos.Select(p => UserDrugPhotoResource.MapToResource(p)).ToList() :
                new List<UserDrugPhotoResource>(){new UserDrugPhotoResource(){Id=0 , APIPath="/images/Drug.webp"}},
            UserId = ud.UserId
        };
    }

}
