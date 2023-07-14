using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterNewUserAsync(ApplicationUser user);
    Task<AuthResult> LoginAsync(ApplicationUser user , bool IsAdmin);
    Task<RoleResult> AddRoleToUser(string userId, string RoleName);
    Task<RoleResult> RemoveRoleFromUser(string userId, string roleName);
    Task<ICollection<string?>> GetAllRoles();
}
