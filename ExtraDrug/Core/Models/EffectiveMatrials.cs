using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Core.Models;

public class EffectiveMatrials
{
    public int assem { get; set; }
    public int Id { get; set; }
    [StringLength(150)]
    public string Name { get; set; }
    public ICollection<Drug> InDrugs { get; set; } = new List<Drug>();

}
