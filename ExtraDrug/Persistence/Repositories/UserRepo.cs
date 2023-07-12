using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
}
