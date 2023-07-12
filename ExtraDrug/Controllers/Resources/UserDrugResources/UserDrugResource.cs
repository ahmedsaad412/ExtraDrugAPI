using ExtraDrug.Controllers.Resources.DrugResources;
using ExtraDrug.Core.Models;

namespace ExtraDrug.Controllers.Resources.UserDrugResources;

public class UserDrugResource
{
    public DateTime ExpireDate { get; set; }
    public int Quantity { get; set; }
    public double CoordsLongitude { get; set; }
    public double CoordsLatitude { get; set; }
    public DrugResource? Drug { get; set; }

    public static UserDrugResource MapToResource(UserDrug ud)
    {
        return new UserDrugResource()
        {
            CoordsLatitude = ud.CoordsLatitude,
            CoordsLongitude = ud.CoordsLongitude,
            ExpireDate = ud.ExpireDate,
            Quantity = ud.Quantity,
            Drug = ud.Drug is not null ? DrugResource.MapToResource(ud.Drug) : null
        };
    }

}
