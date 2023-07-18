using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.DrugRequestResources;

public class AddDrugRequestResource
{
    [Required]
    public string DonorId { get; set; }
    [Required]
    public ICollection<AddRequestItemResource> RequestItems { get; set; } = new List<AddRequestItemResource>();

    public DrugRequest MapToModel()
    {
        return new DrugRequest()
        {
            DonorId = DonorId,
            RequestItems = RequestItems.Select(rir => rir.MapToModel()).ToList()
        };
    }
}
