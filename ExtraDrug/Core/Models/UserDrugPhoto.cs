using System.ComponentModel.DataAnnotations;

namespace ExtraDrug.Core.Models;

public class UserDrugPhoto

{
    public int Id { get; set; }
    [Required]
    [StringLength(256)]
    public string APIPath { get; set; } = string.Empty;
    [Required]
    [StringLength(512)]
    public string SysPath { get; set; } = string.Empty;

    public int UserDrugId { get; set; }
    public UserDrug UserDrug { get; set; }

}
