using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.DrugResources;

public class SaveDrugResource
{
    public int Id { get; set; }

    [ MinLength(2) , MaxLength(100)]
    public string? Ar_Name { get; set; }

    [ Required , MinLength(2) , MaxLength(100)]
    public string? En_Name { get; set; }
    
    [Required , MinLength(2) , MaxLength(100)]
    public string? Parcode { get; set; }
    
    [ MaxLength(256)]
    public string? Purpose { get; set; }

    [Required]
    public bool IsTradingPermitted { get; set; }
    
    public int? CompanyId { get; set; }

    [Required]
    public int TypeId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public IEnumerable<NameAndIdResource> EffectiveMatrials { get; set; } = new List<NameAndIdResource>();

    public Drug MapToModel()
    {
        return new Drug() {
            Id=Id,
            Ar_Name = Ar_Name,
            En_Name = En_Name,
            Parcode = Parcode,
            Purpose = Purpose,
            IsTradingPermitted = IsTradingPermitted,
            CompanyId = CompanyId,
            TypeId = TypeId,
            CategoryId = CategoryId,
            EffectiveMatrials = EffectiveMatrials.Select(e => e.MapToModel<EffectiveMatrial>()).ToList()
        };
    } 

}
