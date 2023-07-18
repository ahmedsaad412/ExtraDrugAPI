using ExtraDrug.Controllers.Resources.UserDrugResources;
using ExtraDrug.Core.Models;

namespace ExtraDrug.Controllers.Resources.DrugRequestResources;

public class RequestItemResource
{
    public int Quantity { get; set; }
    public UserDrugResource UserDrug { get; set; }

    public static RequestItemResource MapToResource(RequestItem ri)
    {
        return new RequestItemResource() 
        {
           Quantity = ri.Quantity,  
           UserDrug = UserDrugResource.MapToResource(ri.UserDrug)
        };
    }
}
