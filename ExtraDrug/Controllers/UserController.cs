using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.UserResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExtraDrug.Controllers;

[Route("api/[controller]")]

[ApiController]
[ValidateModel]
[ExceptionHandler]
public class UserController : ControllerBase
{
    private readonly ResponceBuilder _responceBuilder;
    private readonly IUserRepo _userRepo;

    public UserController(ResponceBuilder responceBuilder, IUserRepo userRepo)
    {
        _responceBuilder = responceBuilder;
        _userRepo = userRepo;
    }
   

    #region User Endpoints
    [HttpGet("{id}")]  
    [Authorize(Roles ="User")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var res = await _userRepo.GetById(id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Not Found",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "User Data fetched",
            data: UserResource.MapToResource(res.Data)
           ));
    }


    [HttpGet("Profile")]
    [Authorize]
    public async Task<IActionResult> GetUserProfile()
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();
        var res = await _userRepo.GetById(userIdFromToken);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Not Found",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "User Data fetched",
            data: UserResource.MapToResource(res.Data)
           ));
    }



    [Authorize(Roles ="Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepo.GetAll();
        return Ok(_responceBuilder.CreateSuccess(
          message: "Users Data fetched",
          data: users.Select(UserResource.MapToResource).ToList()
         ));
    }


    [HttpPatch("photo")]
    [Authorize]
    public async Task<IActionResult> UploadUserPhoto([FromForm] IFormFile file)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var res = await _userRepo.UploadUserPhoto(userIdFromToken, file);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "photo Added",
            data: UserResource.MapToResource(res.Data)
            ));
    }

    [HttpPatch("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangeUserPassword(ChangePasswordResource chPass)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var res = await _userRepo.ChangeUserPassword(userIdFromToken , chPass.OldPassword , chPass.NewPassword);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Can't change user password.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "Password Changed",
            data: UserResource.MapToResource(res.Data)
            ));

    }
     

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditUser(EditUserResource edr)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var res = await _userRepo.EditUser(userIdFromToken, edr.MapToModel());
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "Can't Update user Data.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "User Data Updated",
            data: UserResource.MapToResource(res.Data)
            ));

    }

    #endregion
}

