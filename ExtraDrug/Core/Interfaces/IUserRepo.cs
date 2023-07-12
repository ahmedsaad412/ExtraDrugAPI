using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;


public interface IUserRepo
{
    Task<RepoResult<ApplicationUser>> GetById(string id);
    Task<ICollection<ApplicationUser>> GetAll();
    Task<RepoResult<ApplicationUser>> AddDrugToUser(UserDrug ud);
}
