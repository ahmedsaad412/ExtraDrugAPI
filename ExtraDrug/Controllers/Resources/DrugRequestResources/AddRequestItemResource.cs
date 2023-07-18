using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.DrugRequestResources;

public class AddRequestItemResource
{
    [Required]
    public int UserDrugId { get; set; }
    [Required, Range(minimum:1,maximum:1000)]
    public int Quantity { get; set; }

    public RequestItem MapToModel()
    {
        return new RequestItem() {
            UserDrugId =  UserDrugId,
            Quantity = Quantity
        };
    }
}
