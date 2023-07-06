using ExtraDrug.Controllers.Resources;
using ExtraDrug.Controllers.Resources.Auth;
using ExtraDrug.Controllers.Resources.User;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Controllers.Attributes;

using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("/register")]
    [ValidateModel]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserResource cr)
    {
        //TODO:: change the responce of validation to use this code. 
        if (!ModelState.IsValid)
            return BadRequest(
                new ErrorResponce()
                {
                    Errors = ModelState.SelectMany(e => e.Value.Errors.Select(e => e.ErrorMessage)).ToList(),
                    Message = "Invalid User Data."
                });

        var res = await authService.RegisterNewUser(cr.MapToModel());
        if (!res.IsSucceeded || res.User is null)
        {
            return BadRequest(
               new ErrorResponce()
               {
                   Errors = res.Errors,
                   Message = "Invalid User Data."
               });
        }

        
        return Created("",new AuthResource()
        {
            Username = res.User.UserName,
            Email = res.User.Email ,
            Roles = res.UserRoles,
            Token = res.JwtToken,
            ExpiresOn = res.ExpiresOn
        });
    }
}
