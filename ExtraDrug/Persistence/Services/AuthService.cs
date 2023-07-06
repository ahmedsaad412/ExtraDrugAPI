using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExtraDrug.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly JWT jwtSettings;

    public AuthService(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IOptions<JWT> _jwtSettings )
    {
        userManager = _userManager;
        roleManager = _roleManager;
        jwtSettings = _jwtSettings.Value;
    }

    public async Task<AuthResult> RegisterNewUserAsync(ApplicationUser user)
    {
        if (user.Email is null || await userManager.FindByEmailAsync(user.Email) is not null)
        {
            return new AuthResult() { IsSucceeded = false, Errors = new string[] { "This Email is Already Used." } };
        }

        if (user.UserName is null || await userManager.FindByNameAsync(user.UserName) is not null)
        {
            return new AuthResult() { IsSucceeded = false, Errors = new string[] { "This Username is Already Used." } };
        }

        if (user.Password is null) return new AuthResult() { IsSucceeded = false, Errors = new string[] { "Password can't be empty." } };

        var res = await userManager.CreateAsync(user, user.Password);
        if (!res.Succeeded)
        {
            return new AuthResult()
            {
                IsSucceeded = false,
                Errors = res.Errors.Select(e => e.Description).ToList()
            };
        }
        await userManager.AddToRoleAsync(user, "User");
        return new AuthResult() {
            User = await userManager.FindByNameAsync(user.UserName),
            UserRoles = new string[] { "User" },
            Errors = null,
            IsSucceeded = true,
            JwtToken = await CreateJwtToken(user),
            ExpiresOn = DateTime.Now.AddDays(jwtSettings.DurationInDays)
        };

    }
    public async Task<AuthResult> LoginAsync(ApplicationUser _userData)
    {
        if (_userData is null ||_userData.Email is null || _userData.Password is null ) 
            return new AuthResult() { Errors = new string[] { "Invalid Email or Password" } };
        var user = await userManager.FindByEmailAsync(_userData.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user , _userData.Password))
            return new AuthResult() { Errors = new string[] { "Incorrect Email or Password" } };
        var jwtToken = await CreateJwtToken(user);
        var rolesList = await userManager.GetRolesAsync(user);
        var roles = rolesList.ToList();
        return new AuthResult() {
            IsSucceeded = true,
            User = user,
            JwtToken = jwtToken,
            UserRoles = roles,
            ExpiresOn = DateTime.Now.AddDays(jwtSettings.DurationInDays)
        };
    }
    private async Task<string> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        var rolesClaims = new List<Claim>();
        foreach (var role in roles)
        {
            rolesClaims.Add(new Claim("role", role));
        }
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub , user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email , user.Email),
            new Claim("uid" , user.Id),

        }.Union(rolesClaims).Union(userClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtTokenObj = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwtSettings.DurationInDays),
                signingCredentials: signingCredentials
            );
        return new JwtSecurityTokenHandler().WriteToken(jwtTokenObj);
    }
    public async Task<RoleResult> AddRoleToUser(string userId , string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if(user is null||!await roleManager.RoleExistsAsync(roleName))
        {
            return new RoleResult()
            {
                IsSucceeded = false ,
                Errors = new string[] {"User Or Role NotFound."}
            };
        } 
        if(await userManager.IsInRoleAsync(user , roleName))
        {
            return new RoleResult()
            {
                IsSucceeded = false,
                Errors = new string[] { "User Already in this Role." }
            };
        }

        var res = await userManager.AddToRoleAsync(user , roleName);

        if (!res.Succeeded)
        {
            return new RoleResult()
            {
                IsSucceeded = false,
                Errors = res.Errors.Select(e => e.Description).ToList()
            };
        }
        return new RoleResult() { IsSucceeded = true };
    }
    public async Task<RoleResult> RemoveRoleFromUser(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null || !await roleManager.RoleExistsAsync(roleName))
        {
            return new RoleResult()
            {
                IsSucceeded = false,
                Errors = new string[] { "User Or Role NotFound." }
            };
        }
        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            return new RoleResult()
            {
                IsSucceeded = false,
                Errors = new string[] { "User Not Assigned to this Role." }
            };
        }

        var res = await userManager.RemoveFromRoleAsync(user, roleName);

        if (!res.Succeeded)
        {
            return new RoleResult()
            {
                IsSucceeded = false,
                Errors = res.Errors.Select(e => e.Description).ToList()
            };
        }
        return new RoleResult() { IsSucceeded = true };
    }

    public async Task<ICollection<string?>> GetAllRoles()
    {
        return  await roleManager.Roles.Select(r => r.Name).ToListAsync();
    }
}
