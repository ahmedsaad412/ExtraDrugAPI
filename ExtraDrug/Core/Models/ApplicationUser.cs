using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExtraDrug.Core.Models;

public class ApplicationUser:IdentityUser
{
    [Required ,StringLength(50)]
    public string FirstName { get; set; } = "";
    [Required, StringLength(50)]
    public string LastName { get; set; } = "";
    [NotMapped]
    public string? Password { get; set; }

    public ICollection<UserDrug> UserDrugs { get; set; } = new List<UserDrug>();
    [NotMapped]
    public ICollection<string> Roles { get; set;  } = new List<string>();
    [StringLength(256)]
    public string? PhotoAPIPath { get; set; } 
    [StringLength(512)]
    public string? PhotoSysPath { get; set; } 
    public bool IsDeleted { get; set; } = false;


}
