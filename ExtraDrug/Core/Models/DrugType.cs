using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Core.Models;

public class DrugType
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    public ICollection<Drug> Drugs { get; set; } = new List<Drug>();

}
