using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Helpers;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ExtraDrug.Persistence.Repositories;

public class UserRepo : IUserRepo
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _ctx;
    private readonly IDrugRepo _drugRepo;
    private readonly RepoResultBuilder<ApplicationUser> _repoResultBuilder;
    private readonly PhotoSettings _photoSettings;
    private readonly IFileService _fileService;
    private readonly IHostEnvironment _host;
    private readonly string USERS_DRUGS_FOLDER = "Users_Drugs";
    private readonly string USERS_PHOTOS_FOLDER = "Users";


    public UserRepo(UserManager<ApplicationUser> userManager ,
        AppDbContext ctx , 
        IDrugRepo drugRepo ,
        RepoResultBuilder<ApplicationUser> repoResultBuilder ,
        IOptions<PhotoSettings> photoSettings,
        IFileService fileService, 
        IHostEnvironment host

        )
    {
        _userManager = userManager;
        _ctx = ctx;
        _drugRepo = drugRepo;
        _repoResultBuilder = repoResultBuilder;
        _photoSettings = photoSettings.Value;
        _fileService = fileService;
        _host = host;
    }

    public async Task<ICollection<ApplicationUser>> GetAll()
    {
        var users =  await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            user.Roles = await _userManager.GetRolesAsync(user);
        }
        return users;
    }

    public async Task<RepoResult<ApplicationUser>> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null ) return _repoResultBuilder.Failuer( new[] { "User Not Found" });
        await _ctx.Entry(user)
            .Collection(u => u.UserDrugs)
            .Query()
            .Include(u => u.Drug)
                .ThenInclude(d => d.Company)
            .Include(u => u.Drug)
                .ThenInclude(d => d.DrugType)
            .Include(u => u.Drug)
                .ThenInclude(d => d.DrugCategory)
            .Include(u => u.Drug)
                .ThenInclude(d => d.EffectiveMatrials)
            .Include(ud => ud.Photos)
            .Where(u => u.ExpireDate >  DateTime.UtcNow  && u.Drug != null && u.Drug.IsTradingPermitted)
            .LoadAsync();
        user.Roles = await _userManager.GetRolesAsync(user);
        return new RepoResult<ApplicationUser>() { Errors = null, IsSucceeded = true, Data = user }; ;
    }

    public async Task<RepoResult<ApplicationUser>> AddDrugToUser(UserDrug ud)
    {
        var userRes = await GetById(ud.UserId);
        if (!userRes.IsSucceeded || userRes.Data is null) return userRes;
        var drugRes = await _drugRepo.GetDrugById(ud.DrugId , includeData:true);
        if (!drugRes.IsSucceeded || drugRes.Data is null) return _repoResultBuilder.Failuer(new[] {"Drug Not Found"});
        var user = userRes.Data;
        ud.User = user;
        ud.Drug = drugRes.Data; 
        user.UserDrugs.Add(ud);
        await _ctx.SaveChangesAsync();
        return _repoResultBuilder.Success(user);
    }

    public async Task<RepoResult<ApplicationUser>> DeleteDrugFromUser(string userId , int userDrugId)
    {
        var ud_from_Db = await _ctx.UsersDrugs.Include(ud => ud.Photos).SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _repoResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _repoResultBuilder.Failuer(new[] { "This User did not Own this Drug" });
        foreach (var photo in ud_from_Db.Photos)
        {
            _fileService.RemoveFile(photo.SysPath);
        }
        _ctx.Remove(ud_from_Db);
        await _ctx.SaveChangesAsync();
        return await GetById(userId);

    }

    public async Task<RepoResult<ApplicationUser>> UpdateDrugOwnedByUser(string userId, int userDrugId, UserDrug ud)
    {
        
        var ud_from_Db = await _ctx.UsersDrugs.SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _repoResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _repoResultBuilder.Failuer(new[] { "This User did not Own this Drug" });

        ud_from_Db.CoordsLatitude = ud.CoordsLatitude;
        ud_from_Db.CoordsLongitude = ud.CoordsLongitude;
        ud_from_Db.Quantity = ud.Quantity;
        ud_from_Db.ExpireDate = ud.ExpireDate;

        await _ctx.SaveChangesAsync();
        return await GetById(userId);
    }

    public async Task<RepoResult<ApplicationUser>> UploadUserDrugPhoto(string userId , int userDrugId , IFormFile file)
    {
        var ud_from_Db = await _ctx.UsersDrugs.SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _repoResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _repoResultBuilder.Failuer(new[] { "This User did not Own this Drug" });

        var validationRes = _photoSettings.ValidateFile(file);
        if(!validationRes.IsSucceeded) return _repoResultBuilder.Failuer(validationRes.Errors);
        var UploadFolderPath = Path.Combine(_host.ContentRootPath, "wwwroot", "uploads", USERS_DRUGS_FOLDER);

        var fileName = await _fileService.AddStaticFile(file, UploadFolderPath);

        var photo = new UserDrugPhoto()
        {
            APIPath = $"/uploads/{USERS_DRUGS_FOLDER}/{fileName}",
            SysPath = Path.Combine(UploadFolderPath, fileName),
            UserDrugId = ud_from_Db.Id
        };

        _ctx.UserDrugsPhotos.Add(photo);
        await _ctx.SaveChangesAsync();
        return await GetById(userId); 
    }

    public async Task<RepoResult<ApplicationUser>> DeletePhotoFromUserDrug(string userId, int userDrugId, int photoId)
    {
        var ud_from_Db = await _ctx.UsersDrugs
            .Include(ud => ud.Photos)
            .SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _repoResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _repoResultBuilder.Failuer(new[] { "This User did not Own this Drug" });
        var photo = ud_from_Db.Photos.SingleOrDefault(p => p.Id == photoId);
        if(photo is null) return _repoResultBuilder.Failuer(new[] { "Photo Not Found" });

        var res = _fileService.RemoveFile(photo.SysPath);
        if(!res) return _repoResultBuilder.Failuer(new[] { "Can't Delete the photo" });
        _ctx.Remove(photo);
        await _ctx.SaveChangesAsync();
        return await GetById(userId);
    }

    public async Task<RepoResult<ApplicationUser>> UploadUserPhoto(string userId, IFormFile file)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return _repoResultBuilder.Failuer(new[] { "User Not Found" });
        if(user.PhotoAPIPath is not null && user.PhotoSysPath is not null)
        {
            var res = _fileService.RemoveFile(user.PhotoSysPath);
            if (!res) return _repoResultBuilder.Failuer(new[] { "Can't Delete the Old photo" });
            user.PhotoAPIPath = null;
            user.PhotoSysPath = null; 
        }
        var validationRes = _photoSettings.ValidateFile(file);
        if (!validationRes.IsSucceeded) return _repoResultBuilder.Failuer(validationRes.Errors);
        var UploadFolderPath = Path.Combine(_host.ContentRootPath, "wwwroot", "uploads", USERS_PHOTOS_FOLDER);

        var fileName = await _fileService.AddStaticFile(file, UploadFolderPath);

        user.PhotoSysPath = Path.Combine(UploadFolderPath, fileName);
        user.PhotoAPIPath = $"/uploads/{USERS_PHOTOS_FOLDER}/{fileName}";

        await _ctx.SaveChangesAsync();  
        return await GetById(userId);
    }

    public async Task<RepoResult<ApplicationUser>> ChangeUserPassword(string userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return _repoResultBuilder.Failuer(new[] { "User Not Found" });
        var res = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);   
        if (!res.Succeeded)
        {
            return _repoResultBuilder.Failuer(res.Errors.Select(e=>e.Description).ToList());
        }
        return await GetById(userId);
    }

    public async Task<RepoResult<ApplicationUser>> EditUser(string userId, ApplicationUser userNewData)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return _repoResultBuilder.Failuer(new[] { "User Not Found" });
        
        var userWithSameName =  await _userManager.Users.Where(u => u.Id != userId).SingleOrDefaultAsync( u => u.UserName == userNewData.UserName);
        if(userWithSameName is not null) return _repoResultBuilder.Failuer(new[] { "UserName is Already Used." });

        var userWithSamePhoneNumber = await _userManager.Users.Where(u => u.Id != userId).SingleOrDefaultAsync(u => u.PhoneNumber == userNewData.PhoneNumber);
        if (userWithSameName is not null) return _repoResultBuilder.Failuer(new[] { "Phone Number is Already Used." });


        user.FirstName = userNewData.FirstName;
        user.LastName = userNewData.LastName;
        user.PhoneNumber = userNewData.PhoneNumber;
        user.UserName = userNewData.UserName;

        await _ctx.SaveChangesAsync();
        return await GetById(userId);
    }
}
