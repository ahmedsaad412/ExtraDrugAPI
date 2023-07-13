namespace ExtraDrug.Core.Models;

public class UserDrug
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int  DrugId{ get; set; }
    public ApplicationUser? User { get; set; }
    public Drug? Drug { get; set; }
    public DateTime ExpireDate { get; set; }
    public int Quantity { get; set; }
    public double CoordsLongitude { get; set; } 
    public double CoordsLatitude { get; set; }
    public ICollection<UserDrugPhoto> Photos { get; set; } = new List<UserDrugPhoto>();
}
