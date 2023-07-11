using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExtraDrug.Persistence.Repositories;

public class UserRepo : IUserRepo
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepo(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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

    public async Task<ApplicationUser?> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null ) return null; 
        user.Roles = await _userManager.GetRolesAsync(user);
        return user;
    }

}
