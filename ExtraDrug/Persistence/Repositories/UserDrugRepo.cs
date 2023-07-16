using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Helpers;
using ExtraDrug.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ExtraDrug.Persistence.Repositories;

public class UserDrugRepo:IUserDrugRepo
{
    private readonly AppDbContext _ctx;
    private readonly IDrugRepo _drugRepo;
    private readonly IUserRepo _userRepo;
    private readonly RepoResultBuilder<UserDrug> _userDrugResultBuilder;
    private readonly PhotoSettings _photoSettings;
    private readonly IFileService _fileService;
    private readonly IHostEnvironment _host;
    private readonly string USERS_DRUGS_FOLDER = "Users_Drugs";


    public UserDrugRepo(
        AppDbContext ctx,
        IDrugRepo drugRepo,
        IUserRepo userRepo,
        RepoResultBuilder<UserDrug> userDrugResultBuilder,
        IOptions<PhotoSettings> photoSettings,
        IFileService fileService,
        IHostEnvironment host

        )
    {
        _ctx = ctx;
        _drugRepo = drugRepo;
        _userRepo = userRepo;
        _userDrugResultBuilder = userDrugResultBuilder;
        _photoSettings = photoSettings.Value;
        _fileService = fileService;
        _host = host;
    }


    public async Task<RepoResult<UserDrug>> GetUserDrugById(int id)
    {
        var userDrug = await _ctx.UsersDrugs
            .Include(ud => ud.Drug).ThenInclude(d => d.Company)
            .Include(ud => ud.Drug).ThenInclude(d => d.DrugCategory)
            .Include(ud => ud.Drug).ThenInclude(d => d.DrugType)
            .Include(ud => ud.Photos)
            .SingleOrDefaultAsync(ud => ud.Id == id);
        if (userDrug is null)
            return _userDrugResultBuilder.Failuer(new[] { "UserDrug Id Invalid, UserDrug NotFound." });
        return _userDrugResultBuilder.Success(userDrug);
    }
    public async Task<RepoResult<UserDrug>> AddDrugToUser(UserDrug ud)
    {
        var userRes = await _userRepo.GetById(ud.UserId);
        if (!userRes.IsSucceeded || userRes.Data is null) return _userDrugResultBuilder.Failuer(new[] { "User Not Found" });
        var drugRes = await _drugRepo.GetDrugById(ud.DrugId, includeData: true);
        if (!drugRes.IsSucceeded || drugRes.Data is null) return _userDrugResultBuilder.Failuer(new[] { "Drug Not Found" });
        var user = userRes.Data;
        ud.User = user;
        ud.Drug = drugRes.Data;
        ud.CreatedAt = DateTime.UtcNow;
        user.UserDrugs.Add(ud);
        await _ctx.SaveChangesAsync();
        return _userDrugResultBuilder.Success(ud);
    }
    public async Task<RepoResult<UserDrug>> DeleteDrugFromUser(string userId, int userDrugId)
    {
        var ud_from_Db = await _ctx.UsersDrugs.Include(ud => ud.Photos).SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _userDrugResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _userDrugResultBuilder.Failuer(new[] { "This User did not Own this Drug" });
        foreach (var photo in ud_from_Db.Photos)
        {
            _fileService.RemoveFile(photo.SysPath);
        }
        _ctx.Remove(ud_from_Db);
        await _ctx.SaveChangesAsync();
        return await GetUserDrugById(userDrugId);

    }
    public async Task<RepoResult<UserDrug>> UpdateDrugOwnedByUser(string userId, int userDrugId, UserDrug ud)
    {

        var ud_from_Db = await _ctx.UsersDrugs.SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _userDrugResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _userDrugResultBuilder.Failuer(new[] { "This User did not Own this Drug" });

        ud_from_Db.CoordsLatitude = ud.CoordsLatitude;
        ud_from_Db.CoordsLongitude = ud.CoordsLongitude;
        ud_from_Db.Quantity = ud.Quantity;
        ud_from_Db.ExpireDate = ud.ExpireDate;

        await _ctx.SaveChangesAsync();
        return await GetUserDrugById(userDrugId);
    }
    public async Task<RepoResult<UserDrug>> UploadUserDrugPhoto(string userId, int userDrugId, IFormFile file)
    {
        var ud_from_Db = await _ctx.UsersDrugs.SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _userDrugResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _userDrugResultBuilder.Failuer(new[] { "This User did not Own this Drug" });

        var validationRes = _photoSettings.ValidateFile(file);
        if (!validationRes.IsSucceeded) return _userDrugResultBuilder.Failuer(validationRes.Errors);
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
        return await GetUserDrugById(userDrugId);
    }
    public async Task<RepoResult<UserDrug>> DeletePhotoFromUserDrug(string userId, int userDrugId, int photoId)
    {
        var ud_from_Db = await _ctx.UsersDrugs
        .Include(ud => ud.Photos)
            .SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _userDrugResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _userDrugResultBuilder.Failuer(new[] { "This User did not Own this Drug" });
        var photo = ud_from_Db.Photos.SingleOrDefault(p => p.Id == photoId);
        if (photo is null) return _userDrugResultBuilder.Failuer(new[] { "Photo Not Found" });

        var res = _fileService.RemoveFile(photo.SysPath);
        if (!res) return _userDrugResultBuilder.Failuer(new[] { "Can't Delete the photo" });
        _ctx.Remove(photo);
        await _ctx.SaveChangesAsync();
        return await GetUserDrugById(userDrugId);
    }

}
