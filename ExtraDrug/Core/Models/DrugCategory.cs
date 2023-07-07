using ExtraDrug.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Core.Models;

public class DrugCategory: INameAndId
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    public ICollection<Drug> Drugs { get; set; } = new List<Drug>();

}
