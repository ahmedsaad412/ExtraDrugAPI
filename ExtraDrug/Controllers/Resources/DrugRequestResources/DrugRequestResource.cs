using ExtraDrug.Controllers.Resources.UserResources;
using ExtraDrug.Core.Models;

namespace ExtraDrug.Controllers.Resources.DrugRequestResources;

public class DrugRequestResource
{
    public int Id { get; set; }
    public UserResource Donor { get; set; }
    public UserResource Receiver { get; set; }
    public string State { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public ICollection<RequestItemResource> RequestItems { get; set; } = new List<RequestItemResource>();

    public static DrugRequestResource MapToResource(DrugRequest dr)
    {
        return new DrugRequestResource()
        {
            Id = dr.Id,
            Donor = UserResource.MapToResource(dr.Donor) ,
            Receiver = UserResource.MapToResource(dr.Receiver),
            State = dr.State.ToString(),
            CreatedAt = dr.CreatedAt,
            LastUpdatedAt = dr.LastUpdatedAt,
            RequestItems = dr.RequestItems.Select(RequestItemResource.MapToResource).ToList()
        };
    }
}
