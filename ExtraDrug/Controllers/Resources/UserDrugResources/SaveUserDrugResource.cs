using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.UserDrugResources;

public class SaveUserDrugResource
{
    [Required]
    public int DrugId { get; set; }
   
    [Required]
    public DateTime ExpireDate { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public double CoordsLongitude { get; set; }
    
    [Required]
    public double CoordsLatitude { get; set; } 
    
    public UserDrug MapToModel()
    {
        return new UserDrug()
        {
            DrugId = DrugId,
            ExpireDate = ExpireDate,
            Quantity = Quantity,
            CoordsLatitude = CoordsLatitude,
            CoordsLongitude = CoordsLongitude
        };
    }
}
