using ExtraDrug.Controllers.Resources;
using ExtraDrug.Controllers.Resources.Auth;
using ExtraDrug.Controllers.Resources.User;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Controllers.Attributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExtraDrug.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService _authService)
    {
        authService = _authService;
    }

    [HttpPost("register")]
    [ValidateModel]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserResource cr)
    {

        var res = await authService.RegisterNewUserAsync(cr.MapToModel());
        if (!res.IsSucceeded || res.User is null)
        {
            return BadRequest(
               new ErrorResponce()
               {
                   Errors = res.Errors,
                   Message = "Invalid User Data."
               });
        }


        return Created("", new AuthResource()
        {
            Username = res.User.UserName,
            Email = res.User.Email,
            Roles = res.UserRoles,
            UserId = res.User.Id,
            Token = res.JwtToken,
            ExpiresOn = res.ExpiresOn
        });
    }

    [HttpPost("login")]
    [ValidateModel]
    public async Task<IActionResult> LoginUser([FromBody] LoginResource lr)
    {
        var res = await authService.LoginAsync(lr.MapToModel());
        if (!res.IsSucceeded || res.User is null)
        {
            return BadRequest(
               new ErrorResponce()
               {
                   Errors = res.Errors,
                   Message = "Authentication Error: Invalid User Data."
               });
        }


        return Ok(new AuthResource()
        {
            Username = res.User.UserName,
            UserId = res.User.Id,
            Email = res.User.Email,
            Roles = res.UserRoles,
            Token = res.JwtToken,
            ExpiresOn = res.ExpiresOn
        });

    }

    [Authorize(Roles = "Admin")]
    [HttpPost("add-role-to-user")]
    [ValidateModel]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserResource ar)
    {
        var res = await authService.AddRoleToUser(ar.UserId , ar.Role);
        if (!res.IsSucceeded )
        {
            return BadRequest(
               new ErrorResponce()
               {
                   Errors = res.Errors,
                   Message = "Invalid User Or Role Data"
               });
        }

        return Ok(new SuccessResponce<object>(){ Message = "Role is added to the user"});
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("remove-role-from-user")]
    [ValidateModel]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] AddRoleToUserResource ar)
    {
        var res = await authService.RemoveRoleFromUser(ar.UserId, ar.Role);
        if (!res.IsSucceeded)
        {
            return BadRequest(
               new ErrorResponce()
               {
                   Errors = res.Errors,
                   Message = "Invalid User Or Role Data"
               });
        }

        return Ok(new SuccessResponce<object>() { Message = "Role is removed from the user" });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles =  await authService.GetAllRoles();
        return Ok(new SuccessResponce<ICollection<string?>?>()
        {
            Message = "All Roles",
            Data = roles
        });
    }
}
