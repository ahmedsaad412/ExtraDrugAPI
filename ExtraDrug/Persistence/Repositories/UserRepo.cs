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
    private readonly RepoResultBuilder<ApplicationUser> _repoResultBuilder;
    private readonly PhotoSettings _photoSettings;
    private readonly IFileService _fileService;
    private readonly IHostEnvironment _host;
    private readonly string USERS_PHOTOS_FOLDER = "Users";


    public UserRepo(UserManager<ApplicationUser> userManager ,
        AppDbContext ctx , 
        IDrugRepo drugRepo ,
        RepoResultBuilder<ApplicationUser> repoResultBuilder ,
        RepoResultBuilder<UserDrug> userDrugResultBuilder,
        IOptions<PhotoSettings> photoSettings,
        IFileService fileService, 
        IHostEnvironment host

        )
    {
        _userManager = userManager;
        _ctx = ctx;
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
