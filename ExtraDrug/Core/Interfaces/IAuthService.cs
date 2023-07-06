using ExtraDrug.Core.Models;

namespace ExtraDrug.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterNewUser(ApplicationUser user);
}
