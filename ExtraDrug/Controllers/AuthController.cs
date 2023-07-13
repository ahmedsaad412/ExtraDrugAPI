using ExtraDrug.Controllers.Resources.Auth;
using ExtraDrug.Controllers.Resources.User;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Controllers.Attributes;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExtraDrug.Persistence.Services;

namespace ExtraDrug.Controllers;

[Route("api/[controller]")]
[ApiController]
[ValidateModel]
[ExceptionHandler]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ResponceBuilder _responceBuilder;

    public AuthController(IAuthService authService , ResponceBuilder responceBuilder)
    {
        _authService = authService;
        _responceBuilder = responceBuilder;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserResource cr)
    {

        var res = await _authService.RegisterNewUserAsync(cr.MapToModel());
        if (!res.IsSucceeded || res.Data is null)
        {
            return BadRequest(_responceBuilder.CreateFailure(message: "Invalid User Data.", errors: res.Errors));  
        }
        //TODO :: put the location header value 
        return Created( "",_responceBuilder.CreateSuccess(data: AuthResource.MapToResource(res), message: "Registered Successfuly"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginResource lr)
    {
        var res = await _authService.LoginAsync(lr.MapToModel());
        if (!res.IsSucceeded || res.Data is null)
        {
            return BadRequest(_responceBuilder.CreateFailure(message: "Authentication Error: Invalid User Data.", errors: res.Errors));
        }
        return Ok(_responceBuilder.CreateSuccess(data: AuthResource.MapToResource(res) , message: "LoggedIn Successfuly "));

    }

    [HttpPost("add-role-to-user")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserResource ar)
    {
        var res = await _authService.AddRoleToUser(ar.UserId , ar.Role);
        if (!res.IsSucceeded )
            return BadRequest(_responceBuilder.CreateFailure(message: "Invalid User Or Role Data", errors: res.Errors));
        return Ok(_responceBuilder.CreateSuccess<object?>(message: "Role is added to the user", data: null));
    }

    [HttpPost("remove-role-from-user")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveRoleFromUser([FromBody] AddRoleToUserResource ar)
    {
        var res = await _authService.RemoveRoleFromUser(ar.UserId, ar.Role);
        if (!res.IsSucceeded)
            return BadRequest(_responceBuilder.CreateFailure(message: "Invalid User Or Role Data", errors: res.Errors));
        return Ok(_responceBuilder.CreateSuccess<object?>(message: "Role is removed from the user", data: null));
    }

    [HttpGet("roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles =  await _authService.GetAllRoles();
        return Ok(_responceBuilder.CreateSuccess(message: "All Roles", data: roles));
    }
}
