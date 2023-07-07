using ExtraDrug.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Controllers.Resources.DrugResources;

public class DrugResource
{
    public int Id { get; set; }

    public string? Ar_Name { get; set; }

    public string? En_Name { get; set; }
    
    public string? Parcode { get; set; }
    
    public string? Purpose { get; set; }

    public bool IsTradingPermitted { get; set; }
    
    public int? CompanyId { get; set; }
    public string? CompanyName { get; set; }


    public int TypeId { get; set; }
    public string? TypeName { get; set; }


    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }


    public IEnumerable<NameAndIdResource> EffectiveMatrials { get; set; } = new List<NameAndIdResource>();

    public static DrugResource MapToResource(Drug d)
    {
        return new DrugResource() {
            Id=d.Id,
            Ar_Name = d.Ar_Name,
            En_Name = d.En_Name,
            Parcode = d.Parcode,
            Purpose = d.Purpose,
            IsTradingPermitted = d.IsTradingPermitted,
            CompanyId = d.CompanyId,
            CompanyName = d.Company?.Name,
            TypeId = d.TypeId,
            TypeName = d.DrugType?.Name,
            CategoryId = d.CategoryId,
            CategoryName = d.DrugCategory?.Name,
            EffectiveMatrials = d.EffectiveMatrials.Select(e => NameAndIdResource.MapToResource(e)).ToList()
        };
    } 

}
