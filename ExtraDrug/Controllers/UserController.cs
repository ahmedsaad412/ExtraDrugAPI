using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.Auth;
using ExtraDrug.Controllers.Resources.UserDrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
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

    [HttpPost("Drugs")]
    [Authorize(Roles = "User")]

    public async Task<IActionResult> AddUserDrug( [FromBody] SaveUserDrugResource udr )
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();
        var ud = udr.MapToModel();
        ud.UserId = userIdFromToken;
        var res = await _userRepo.AddDrugToUser(ud);
        if (! res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message:"User Or Drug Not Found.",
                    errors: res.Errors
                )) ;
        return Ok(_responceBuilder.CreateSuccess(
            message:"drug added",
            data:UserResource.MapToResource(res.Data)
            ));
    }

    [HttpPut("Drugs/{id:int}")]
    [Authorize(Roles ="User")]

    public async Task<IActionResult> UpdateUserDrug([FromRoute] int id,[FromBody] SaveUserDrugResource udr)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var ud = udr.MapToModel();
        var res = await _userRepo.UpdateDrugOwnedByUser(userIdFromToken , id , ud);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Or Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "drug Updated",
            data: UserResource.MapToResource(res.Data)
            ));
    }


    [HttpDelete("Drugs/{id:int}")]
    [Authorize(Roles ="User")]

    public async Task<IActionResult> DeleteUserDrug([FromRoute] int id)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();
        var res = await _userRepo.DeleteDrugFromUser(userIdFromToken, id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Or Drug Not Found.",
                    errors: res.Errors  
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "drug deleted",
            data: UserResource.MapToResource(res.Data)
            ));
    }



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
}

