using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Core.Models;

public class Drug
{
    public int Id { get; set; }
    [StringLength(100)]
    public string? Ar_Name { get; set; }
    [StringLength(100)]
    public string? En_Name { get; set; }
    [StringLength(100)]
    public string? Parcode { get; set; }
    [StringLength(256)]
    public string? Purpose { get; set; }
    public bool IsTradingPermitted { get; set; }
    public int? CompanyId { get; set; }
    public DrugCompany? Company { get; set; }
    public int TypeId { get; set; }
    public DrugType? DrugType { get; set; }
    public int  CategoryId { get; set; }
    public DrugCategory? DrugCategory { get; set; }
    public ICollection<EffectiveMatrials> EffectiveMatrials { get; set; } = new List<EffectiveMatrials>();

}
