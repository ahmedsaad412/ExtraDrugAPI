using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ExtraDrug.Persistence.Repositories;

public class UserRepo : IUserRepo
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _ctx;
    private readonly IDrugRepo _drugRepo;
    private readonly RepoResultBuilder<ApplicationUser> _repoResultBuilder;

    public UserRepo(UserManager<ApplicationUser> userManager  , AppDbContext ctx , IDrugRepo drugRepo ,RepoResultBuilder<ApplicationUser> repoResultBuilder)
    {
        _userManager = userManager;
        _ctx = ctx;
        _drugRepo = drugRepo;
        _repoResultBuilder = repoResultBuilder;
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
        var ud_from_Db = await _ctx.UsersDrugs.SingleOrDefaultAsync(ud => ud.Id == userDrugId);
        if (ud_from_Db is null) return _repoResultBuilder.Failuer(new[] { "User Drug Not Found " });
        if (ud_from_Db.UserId != userId) return _repoResultBuilder.Failuer(new[] { "This User did not Own this Drug" });
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
}
