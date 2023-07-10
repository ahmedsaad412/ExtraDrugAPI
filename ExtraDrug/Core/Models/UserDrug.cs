namespace ExtraDrug.Core.Models;

public class UserDrug
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public int  DrugId{ get; set; }
    public ApplicationUser? User { get; set; }
    public Drug? Drug { get; set; }
    public string Image { get; set; } = "";
    public DateTime ExpireDate { get; set; }
    public DateTime ManufactureDate { get; set; }
    public int Quantity { get; set; }
    public string coordsLongitude { get; set; } = "";
    public string coordsLatitude { get; set; } = "";

}
