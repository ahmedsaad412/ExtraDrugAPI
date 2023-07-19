using ExtraDrug.Controllers.Attributes;
using ExtraDrug.Controllers.Resources.UserDrugResources;
using ExtraDrug.Core.Interfaces;
using ExtraDrug.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;   

namespace ExtraDrug.Controllers;
[Route("api/user/Drugs")]
[ApiController]
[ValidateModel]
[ExceptionHandler]
public class UserDrugController : ControllerBase
{
    private readonly ResponceBuilder _responceBuilder;
    private readonly IUserDrugRepo _userDrugRepo;

    public UserDrugController(ResponceBuilder responceBuilder, IUserDrugRepo userDrugRepo)
    {
        _responceBuilder = responceBuilder;
        _userDrugRepo = userDrugRepo;
    }



    #region User Drug endpoints

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllUserDrugs()
    {
        var res = await _userDrugRepo.GetAllUserDrugs();
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "all Users Drugs Featched",
            data: res.Data.Select(UserDrugResource.MapToResource).ToList()
            )) ;
    }


    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetUserDrugById([FromRoute] int id)
    {
        var res = await _userDrugRepo.GetUserDrugById(id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "User Drug Featched",
            data: UserDrugResource.MapToResource(res.Data)
            ));
    }


    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> AddUserDrug([FromBody] SaveUserDrugResource udr)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var ud = udr.MapToModel();
        ud.UserId = userIdFromToken;
        var res = await _userDrugRepo.AddDrugToUser(ud);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Or Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "drug added",
            data: UserDrugResource.MapToResource(res.Data)
            ));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateUserDrug([FromRoute] int id, [FromBody] SaveUserDrugResource udr)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var ud = udr.MapToModel();
        var res = await _userDrugRepo.UpdateDrugOwnedByUser(userIdFromToken, id, ud);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Or Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "drug Updated",
            data: UserDrugResource.MapToResource(res.Data)
            ));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteUserDrug([FromRoute] int id)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();
        var res = await _userDrugRepo.DeleteDrugFromUser(userIdFromToken, id);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Or Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "drug deleted",
            data: UserDrugResource.MapToResource(res.Data)
            ));
    }

    [HttpPost("{id:int}/photos")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UploadUserDrugPhoto([FromRoute] int id, [FromForm] IFormFile file)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var res = await _userDrugRepo.UploadUserDrugPhoto(userIdFromToken, id, file);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Or Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "photo Added",
            data: UserDrugResource.MapToResource(res.Data)
            ));
    }

    [HttpDelete("{userDrugId:int}/photos/{photoId:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteUserDrugPhoto([FromRoute] int userDrugId, [FromRoute] int photoId)
    {
        string? userIdFromToken = User.FindFirstValue("uid");

        if (userIdFromToken is null)
            return Forbid();

        var res = await _userDrugRepo.DeletePhotoFromUserDrug(userIdFromToken, userDrugId, photoId);
        if (!res.IsSucceeded || res.Data is null)
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Or Drug Not Found.",
                    errors: res.Errors
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "photo Deleted",
            data: UserDrugResource.MapToResource(res.Data)
            ));
    }


    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> SearchByDrugId([FromQuery]int id, [FromQuery]double lat, [FromQuery] double lon )
    {
        var res = await _userDrugRepo.GetAllUserDrugsOfaDrug(id, lat, lon);
        if (!res.IsSucceeded || res.Data is null || res.Data.Count == 0 )
            return NotFound(_responceBuilder.CreateFailure(
                    message: "User Drugs Not Found.",
                    errors: null
                ));
        return Ok(_responceBuilder.CreateSuccess(
            message: "User Drugs Fetched",
            data: res.Data.Select(UserDrugResource.MapToResource)
            ));
    } 
    #endregion

}
