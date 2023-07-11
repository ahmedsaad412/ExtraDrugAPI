using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.Auth;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Core.Models;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace ExtraDrug.Controllers;

[Route("api/[controller]")]

[ApiController]
[ValidateModel]
[ExceptionHandler]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ResponceBuilder _responceBuilder;
    private readonly IUserRepo _userRepo;

    public UserController(ResponceBuilder responceBuilder, IUserRepo userRepo)
    {
        _responceBuilder = responceBuilder;
        _userRepo = userRepo;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userRepo.GetById(id);
        if (user is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Not Found",
                    errors: new[] { "Invalid Id" }
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "User Data fetched",
            data: UserResource.MapToResource(user)
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

