
namespace ExtraDrug.Core.Models;

public class RequestItem
{
    public int UserDrugId { get; set; }
    public int DrugRequestId { get; set; }
    public int Quantity { get; set; }

    public UserDrug UserDrug { get; set; }
    public DrugRequest DrugRequest { get; set; }
}
