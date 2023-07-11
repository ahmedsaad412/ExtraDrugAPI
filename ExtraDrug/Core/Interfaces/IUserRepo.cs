using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Identity;

namespace ExtraDrug.Core.Interfaces;


public interface IUserRepo
{

   
    Task<ApplicationUser?> GetById(string id);
    Task<ICollection<ApplicationUser>> GetAll();

}
