using ExtraDrug.Controllers.Resources.UserDrugResources;
using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;


public interface IUserRepo
{
    Task<RepoResult<ApplicationUser>> GetById(string id);
    Task<ICollection<ApplicationUser>> GetAll();
    Task<RepoResult<UserDrug>> AddDrugToUser(UserDrug ud);
    Task<RepoResult<ApplicationUser>> DeleteDrugFromUser(string userId, int userDrugId);
    Task<RepoResult<ApplicationUser>> UpdateDrugOwnedByUser(string userId, int userDrugId, UserDrug ud);

    Task<RepoResult<UserDrug>> GetUserDrugById(int id);

    Task<RepoResult<ApplicationUser>> DeletePhotoFromUserDrug( string userId, int userDrugId, int photoId);
    
    Task<RepoResult<ApplicationUser>> UploadUserDrugPhoto(string userId, int userDrugId, IFormFile file);

    Task<RepoResult<ApplicationUser>> UploadUserPhoto(string userId, IFormFile file );
    Task<RepoResult<ApplicationUser>> ChangeUserPassword(string userId,string oldPassword , string newPassword);
    Task<RepoResult<ApplicationUser>> EditUser(string userId, ApplicationUser userNewData);
}
