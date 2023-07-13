using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;


public interface IUserRepo
{
    Task<RepoResult<ApplicationUser>> GetById(string id);
    Task<ICollection<ApplicationUser>> GetAll();
    Task<RepoResult<ApplicationUser>> AddDrugToUser(UserDrug ud);
    Task<RepoResult<ApplicationUser>> DeleteDrugFromUser(string userId, int userDrugId);
    Task<RepoResult<ApplicationUser>> UpdateDrugOwnedByUser(string userId, int userDrugId, UserDrug ud);

}
