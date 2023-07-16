using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IUserDrugRepo
{
    Task<RepoResult<UserDrug>> AddDrugToUser(UserDrug ud);
    Task<RepoResult<UserDrug>> DeleteDrugFromUser(string userId, int userDrugId);
    Task<RepoResult<UserDrug>> UpdateDrugOwnedByUser(string userId, int userDrugId, UserDrug ud);
    Task<RepoResult<UserDrug>> GetUserDrugById(int id);
    Task<RepoResult<UserDrug>> DeletePhotoFromUserDrug(string userId, int userDrugId, int photoId);
    Task<RepoResult<UserDrug>> UploadUserDrugPhoto(string userId, int userDrugId, IFormFile file);

}
