using ExtraDrug.Controllers.Resources.UserDrugResources;
using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;


public interface IUserRepo { 
    Task<RepoResult<ApplicationUser>> GetById(string id,bool withTracking);
    Task<RepoResult<ApplicationUser>> GetByIdWithoutDate(string id);
    Task<ICollection<ApplicationUser>> GetAll();
    Task<RepoResult<ApplicationUser>> UploadUserPhoto(string userId, IFormFile file );
    Task<RepoResult<ApplicationUser>> ChangeUserPassword(string userId,string oldPassword , string newPassword);
    Task<RepoResult<ApplicationUser>> EditUser(string userId, ApplicationUser userNewData);
}
