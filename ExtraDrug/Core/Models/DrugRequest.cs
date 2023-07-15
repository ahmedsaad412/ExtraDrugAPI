namespace ExtraDrug.Core.Models;

public class DrugRequest
{
    public int Id { get; set; }
    public string DonorId { get; set; }
    public ApplicationUser Donor { get; set; }
    public string ReceiverId { get; set; }
    public ApplicationUser Receiver { get; set; }
    public RequestState State { get; set; } = RequestState.Pending;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set;}
    public ICollection<RequestItem> RequestItems { get; set; } = new List<RequestItem>();
}
